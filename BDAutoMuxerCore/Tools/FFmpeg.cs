using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDAutoMuxerCore.BDROM;
using DotNetUtils;
using MediaInfoWrapper;
using ProcessUtils;

namespace BDAutoMuxerCore.Tools
{
    public class FFmpeg : BackgroundProcessWorker
    {
        private const string FFmpegExeFilename = "ffmpeg.exe";

        private static readonly Regex FrameRegex = new Regex(@"^frame=(\d+)$");
        private static readonly Regex FpsRegex = new Regex(@"^fps=([\d.]+)$");
        private static readonly Regex TotalSizeRegex = new Regex(@"^total_size=(\d+)$");
        private static readonly Regex OutTimeMsRegex = new Regex(@"^out_time_ms=(\d+)$");
        private static readonly Regex ProgressRegex = new Regex(@"^progress=(\w+)$");

        private readonly string _progressFilePath;
        private readonly TimeSpan _playlistLength;
        private readonly List<string> _inputM2TSPaths;
        private readonly List<Track> _selectedTracks;
        private readonly string _outputMKVPath;

        private long _curFrame;
        private double _curFps;
        private long _curSize;
        private long _curOutTimeMs;

        private readonly BackgroundWorker _progressWorker = new BackgroundWorker();

        public FFmpeg(Playlist playlist, IEnumerable<Track> selectedTracks, string outputMKVPath)
        {
            _playlistLength = playlist.Length;
            _inputM2TSPaths = playlist.StreamClips.Select(clip => clip.FileInfo.FullName).ToList();
            _selectedTracks = selectedTracks.ToList();
            _outputMKVPath = outputMKVPath;

            if (_inputM2TSPaths.Count == 0)
                throw new ArgumentOutOfRangeException("At least one input M2TS file is required.");

            _progressFilePath = Path.GetTempFileName();

            ExePath = AssemblyUtils.GetTempFilePath(GetType(), FFmpegExeFilename);

            var inputFiles = _inputM2TSPaths.Count == 1 ? _inputM2TSPaths[0] : "concat:" + string.Join("|", _inputM2TSPaths);

            // Replace existing files
            Arguments.AddAll("-y");

            // Send progress information to temp file
            Arguments.AddAll("-progress", _progressFilePath);

            Arguments.AddAll("-i", inputFiles);

            // Map selected tracks to output file
            Arguments.AddRange(_selectedTracks.SelectMany(track => new[] {"-map", "0:" + track.Index}));

            // Copy all codecs
            Arguments.AddAll("-c", "copy");

            // Convert Blu-ray LPCM tracks to signed, little endian PCM for MKV.
            // Blu-ray LPCM is signed, big endian, and either 16-, 20-, or 24-bit.
            // FFmpeg only outputs 16- or 24- bit PCM, so 20-bit Blu-ray LPCM needs to be converted to 24-bit PCM.
            Arguments.AddRange(_selectedTracks
                .Where(track => track.Codec == MICodec.LPCM)
                .SelectMany(track => new[] { "-c:a:" + track.IndexOfType, "pcm_s" + (track.BitDepth == 16 ? 16 : 24) + "le" }));

            Arguments.Add(_outputMKVPath);

            ExtractResources();

            BeforeStart += OnBeforeStart;
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
                    while (!reader.EndOfStream)
                    {
                        ParseProgressLine(reader.ReadLine());
                    }
                }
            }
        }

        private void ParseProgressLine(string line)
        {
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

            if (prevProgress != _progress)
                Console.WriteLine("{0}%", _progress.ToString("0.00"));
        }

        private FileStream CreateProgressFileStream()
        {
            return new FileStream(_progressFilePath,
                                  FileMode.Open,
                                  FileAccess.Read,
                                  FileShare.ReadWrite | FileShare.Delete);
        }

        private void ExtractResources()
        {
            try
            {
                File.WriteAllBytes(ExePath, BinTools.ffmpeg);
            }
            catch { }
        }

        public static void Test()
        {
            var bdrom = new BDInfo.BDROM(@"Y:\BD\49123204_BLACK_HAWK_DOWN");
            bdrom.Scan();
            var disc = Disc.Transform(bdrom);
            var playlist = disc.Playlists.FirstOrDefault(playlist1 => playlist1.Filename.StartsWith("00039"));
            var selectedTracks = playlist.Tracks;
            var outputMKVPath = @"Y:\BDAM\out\progress\BLACK_HAWK_DOWN_00039.mpls.mkv";
            var ffmpeg = new FFmpeg(playlist, selectedTracks, outputMKVPath);
            ffmpeg.StartAsync();
        }
    }
}
