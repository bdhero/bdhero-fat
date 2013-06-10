﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BDHero.BDROM;
using BDHero.JobQueue;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpeg : BackgroundProcessWorker
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public long CurFrame { get; private set; }
        public double CurFps { get; private set; }
        public long CurSize { get; private set; }
        public long CurOutTimeMs { get; private set; }

        private readonly BackgroundWorker _progressWorker = new BackgroundWorker();

        public FFmpeg(Job job, Playlist playlist, string outputMKVPath)
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
            SetMovieTitle(job);
            MapSelectedTracks();
            CopyAllCodecs();
            ConvertLPCM();
            SetOutputMKVPath();

            BeforeStart += OnBeforeStart;
            ProgressUpdated += OnProgressUpdated;
            Exited += (state, code, time) => OnExited(state, code, job.SelectedReleaseMedium, playlist, _selectedTracks, outputMKVPath);
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

        private void SetMovieTitle(Job job)
        {
            var title = job.Disc.SanitizedTitle;
            var releaseMedium = job.SelectedReleaseMedium;
            if (releaseMedium != null)
            {
                var movie = releaseMedium as Movie;
                var tvShow = releaseMedium as TVShow;
                if (movie != null)
                {
                    title = string.Format("{0} ({1})", movie.Title, movie.ReleaseYear);
                }
                else if (tvShow != null)
                {
                    title = string.Format("{0} - {1}, season {2}, episode {3} ({4})",
                                          tvShow.SelectedEpisode.Title,
                                          tvShow.Title,
                                          tvShow.SelectedEpisode.SeasonNumber,
                                          tvShow.SelectedEpisode.EpisodeNumber,
                                          tvShow.SelectedEpisode.ReleaseDate.ToString("yyyy'-'MM'-'dd"));
                }
                else
                {
                    title = releaseMedium.Title;
                }
            }
            Arguments.AddAll("-metadata", "title=" + title);
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
                           "-metadata:s:" + i, "language=" + track.Language.ISO_639_2,
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
#if FALSE
            Console.Write("\r{0}", progressState);
#endif
        }

        private void OnBeforeStart(object sender, EventArgs eventArgs)
        {
            _progressWorker.DoWork += ProgressWorkerOnDoWork;
            _progressWorker.RunWorkerAsync();
            Logger.DebugFormat("\"{0}\" {1}", ExePath, Arguments);
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
                CurFrame = long.Parse(FrameRegex.Match(line).Groups[1].Value);
            else if (FpsRegex.IsMatch(line))
                CurFps = double.Parse(FpsRegex.Match(line).Groups[1].Value);
            else if (TotalSizeRegex.IsMatch(line))
                CurSize = long.Parse(TotalSizeRegex.Match(line).Groups[1].Value);
            else if (OutTimeMsRegex.IsMatch(line))
                CurOutTimeMs = long.Parse(OutTimeMsRegex.Match(line).Groups[1].Value) / 1000;

            var prevProgress = _progress;

            _progress = 100 * (CurOutTimeMs / _playlistLength.TotalMilliseconds);
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
            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            var ffmpegAssemblyDir = Path.GetDirectoryName(assemblyPath);
            ExePath = Path.Combine(ffmpegAssemblyDir, FFmpegExeFilename);
        }

        private static void OnExited(NonInteractiveProcessState processState, int exitCode, ReleaseMedium releaseMedium, Playlist playlist, List<Track> selectedTracks, string outputMKVPath)
        {
            Logger.DebugFormat("FFmpeg exited with state {0} and code {1}", processState, exitCode);
//            if (processState != NonInteractiveProcessState.Completed)
//                return;
#if false
            Console.WriteLine();
            Console.WriteLine("Finished muxing with FFmpeg!");
            Console.WriteLine("Adding metadata with mkvpropedit...");
            var coverArt = Image.FromFile(@"Y:\BDAM\cover-art\black-hawk-down\full.jpg");
#endif
            var coverArt = releaseMedium.CoverArtImages.FirstOrDefault(image => image.IsSelected);
            var coverArtImage = coverArt != null ? coverArt.Image : null;
            var mkvPropEdit = new MkvPropEdit {SourceFilePath = outputMKVPath}
                .RemoveAllTags()
                .AddCoverArt(coverArtImage)
                .SetChapters(playlist.Chapters)
//                .SetDefaultTracksAuto(selectedTracks)
            ;
            mkvPropEdit.Start();
#if false
            Console.WriteLine("********** DONE! **********");
#endif
        }
    }
}
