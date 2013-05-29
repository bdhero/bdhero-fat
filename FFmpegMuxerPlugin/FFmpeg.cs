using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BDHero.BDROM;
using DotNetUtils;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpeg : BackgroundProcessWorker
    {
        private const string FFmpegExeFilename = "ffmpeg.exe";

        private static readonly Regex FrameRegex = new Regex(@"^frame=(\d+)$");
        private static readonly Regex FpsRegex = new Regex(@"^fps=([\d.]+)$");
        private static readonly Regex TotalSizeRegex = new Regex(@"^total_size=(\d+)$");
        private static readonly Regex OutTimeMsRegex = new Regex(@"^out_time_ms=(\d+)$");
        private static readonly Regex ProgressRegex = new Regex(@"^progress=(\w+)$");

        private readonly TimeSpan _playlistLength;
        private readonly List<string> _inputM2TSPaths;
        private readonly List<Track> _selectedTracks;
        private readonly string _outputMKVPath;
        private readonly string _progressFilePath;

        private long _curFrame;
        private double _curFps;
        private long _curSize;
        private long _curOutTimeMs;

        private readonly BackgroundWorker _progressWorker = new BackgroundWorker();

        public FFmpeg(Disc disc, Playlist playlist, string outputMKVPath)
        {
            _playlistLength = playlist.Length;
            _inputM2TSPaths = playlist.StreamClips.Select(clip => clip.FileInfo.FullName).ToList();
            _selectedTracks = playlist.Tracks.Where(track => track.Keep).ToList();
            _outputMKVPath = outputMKVPath;
            _progressFilePath = Path.GetTempFileName();

            VerifyInputPaths();
            VerifySelectedTracks();

            SetExePath();

            ReplaceExistingFiles();
            RedirectProgressToFile();
            SetInputFiles();
            SetMovieTitle(disc.MovieTitle);
            MapSelectedTracks();
            CopyAllCodecs();
            ConvertLPCM();
            SetOutputMKVPath();

            BeforeStart += OnBeforeStart;
            ProgressUpdated += OnProgressUpdated;
            Exited += (state, code, time) => FFmpegOnExited(playlist, _selectedTracks, outputMKVPath);
        }

        private void VerifyInputPaths()
        {
            if (_inputM2TSPaths.Count == 0)
                throw new ArgumentOutOfRangeException("At least one input M2TS file is required.");
        }

        private void VerifySelectedTracks()
        {
            if (_selectedTracks.Count == 0)
                throw new ArgumentOutOfRangeException("At least one track must be selected.");
        }

        private static string GetInputFiles(IList<string> inputM2TsPaths)
        {
            return inputM2TsPaths.Count == 1 ? inputM2TsPaths[0] : "concat:" + string.Join("|", inputM2TsPaths);
        }

        private void ReplaceExistingFiles()
        {
            Arguments.AddAll("-y");
        }

        private void RedirectProgressToFile()
        {
            Arguments.AddAll("-progress", _progressFilePath);
        }

        private void SetInputFiles()
        {
            var inputFiles = GetInputFiles(_inputM2TSPaths);
            Arguments.AddAll("-i", inputFiles);
        }

        private void SetMovieTitle(string movieTitle)
        {
            Arguments.AddAll("-metadata", "title=" + movieTitle);
        }

        private void MapSelectedTracks()
        {
            Arguments.AddRange(_selectedTracks.SelectMany(TrackMetadataArgs));
        }

        private static IEnumerable<string> TrackMetadataArgs(Track track, int i)
        {
            return new[]
                       {
                           "-map", "0:" + track.Index,
                           "-metadata:s:" + track.Index, "language=" + track.Language.ISO_639_2,
                           "-metadata:s:" + i, "title=" + track.Title
                       };
        }

        private void CopyAllCodecs()
        {
            Arguments.AddAll("-c", "copy");
        }

        /// <summary>
        /// Converts Blu-ray LPCM tracks to signed, little endian PCM for MKV.
        /// Blu-ray LPCM is signed, big endian, and either 16-, 20-, or 24-bit.
        /// FFmpeg only outputs 16- or 24- bit PCM, so 20-bit Blu-ray LPCM needs to be converted to 24-bit PCM.
        /// </summary>
        private void ConvertLPCM()
        {
            Arguments.AddRange(_selectedTracks.Where(IsLPCM).SelectMany(LPCMCodecArgs));
        }

        private static bool IsLPCM(Track track)
        {
            return track.Codec == Codec.LPCM;
        }

        private static IEnumerable<string> LPCMCodecArgs(Track track)
        {
            return new[] { "-c:a:" + track.IndexOfType, "pcm_s" + (track.BitDepth == 16 ? 16 : 24) + "le" };
        }

        private void SetOutputMKVPath()
        {
            Arguments.Add(_outputMKVPath);
        }

        private void OnProgressUpdated(ProgressState progressState)
        {
            Console.WriteLine("{0}: {1}% - {2} elapsed, {3} remaining",
                progressState.ProcessState,
                progressState.PercentComplete.ToString("0.000"),
                progressState.TimeElapsed,
                progressState.TimeRemaining);
        }

        private void OnBeforeStart(object sender, EventArgs eventArgs)
        {
            _progressWorker.DoWork += ProgressWorkerOnDoWork;
            _progressWorker.RunWorkerAsync();
        }

        private void ProgressWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            using (var stream = CreateProgressFileStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    while (KeepParsingProgress)
                    {
                        ParseProgressLine(reader.ReadLine());
                    }
                }
                Console.WriteLine("{0}% (FINISHED!)", _progress.ToString("0.00"));
            }
        }

        private bool KeepParsingProgress
        {
            get
            {
                return (_progress < 100) &&
                       (State == NonInteractiveProcessState.Ready ||
                        State == NonInteractiveProcessState.Running ||
                        State == NonInteractiveProcessState.Paused);
            }
        }

        private void ParseProgressLine(string line)
        {
            if (line == null)
            {
                Thread.Sleep(500);
                return;
            }
            if (FrameRegex.IsMatch(line))
                _curFrame = long.Parse(FrameRegex.Match(line).Groups[1].Value);
            else if (FpsRegex.IsMatch(line))
                _curFps = double.Parse(FpsRegex.Match(line).Groups[1].Value);
            else if (TotalSizeRegex.IsMatch(line))
                _curSize = long.Parse(TotalSizeRegex.Match(line).Groups[1].Value);
            else if (OutTimeMsRegex.IsMatch(line))
                _curOutTimeMs = long.Parse(OutTimeMsRegex.Match(line).Groups[1].Value) / 1000;

            var prevProgress = _progress;

            _progress = 100 * (_curOutTimeMs / _playlistLength.TotalMilliseconds);
            _progress = Math.Min(_progress, 100);

            if ("progress=end" == line)
                _progress = 100;
        }

        private FileStream CreateProgressFileStream()
        {
            return new FileStream(_progressFilePath,
                                  FileMode.Open,
                                  FileAccess.Read,
                                  FileShare.ReadWrite | FileShare.Delete);
        }

        private void SetExePath()
        {
//            ExePath = AssemblyUtils.GetTempFilePath(GetType(), FFmpegExeFilename);
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            ExePath = Path.Combine(assemblyDir, FFmpegExeFilename);
        }

#if false
        public static void Test(string bdromDir, string playlistFilename, string outputMKVPath)
        {
            // Step 1: Scan BD-ROM
            var bdrom = new BDInfo.BDROM(bdromDir);
            bdrom.ScanProgress += BDROMOnScanProgress;
            bdrom.Scan();
            var disc = Disc.Transform(bdrom);

            // Step 2: Search BDAM DB
            // ...

            // Step 3: Search TMDb
            // ...

            // Step 4: User selection
            var playlist = disc.Playlists.FirstOrDefault(mpls => mpls.Filename.Equals(playlistFilename, StringComparison.InvariantCultureIgnoreCase));
            var selectedTracks = playlist.Tracks.Where(track => track.Language == Language.FromCode("eng") && track.Codec.IsKnown).ToList();
            foreach (var track in selectedTracks)
                track.Keep = true;

            // Step 5: Mux selected tracks to MKV
            var ffmpeg = new FFmpeg(disc, playlist, outputMKVPath);
            ffmpeg.StartAsync();
//            FFmpegOnExited(playlist, selectedTracks, outputMKVPath);
        }

        private static void BDROMOnScanProgress(BDROMScanProgressState state)
        {
            Console.WriteLine("BDROM: {0}: scanning {1} of {2} ({3}%).  Total: {4} of {5} ({6}%).",
                state.FileType, state.CurFileOfType, state.NumFilesOfType, state.TypeProgress.ToString("0.00"),
                state.CurFileOverall, state.NumFilesOverall, state.OverallProgress.ToString("0.00"));
        }
#endif

        private static void FFmpegOnExited(Playlist playlist, List<Track> selectedTracks, string outputMKVPath)
        {
            Console.WriteLine("Finished muxing with FFmpeg!");
#if false
            Console.WriteLine("Adding metadata with mkvpropedit...");
            var coverArt = Image.FromFile(@"Y:\BDAM\cover-art\black-hawk-down\full.jpg");
            var mkvPropEdit = new MkvPropEdit {SourceFilePath = outputMKVPath}
//                .RemoveAllTags()
                .AddCoverArt(coverArt)
                .SetChapters(playlist.Chapters)
                .SetDefaultTracksAuto(selectedTracks);
            mkvPropEdit.Start();
            Console.WriteLine("********** DONE! **********");
#endif
        }
    }
}
