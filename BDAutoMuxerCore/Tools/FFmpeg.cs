using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using BDAutoMuxerCore.BDROM;
using MediaInfoWrapper;
using ProcessUtils;

namespace BDAutoMuxerCore.Tools
{
    public class FFmpeg : AbstractExternalTool
    {
        private const string FFmpegExeFilename = "ffmpeg.exe";
        private readonly string _ffmpegExePath;
        private readonly List<string> _inputM2TSPaths;
        private readonly List<Track> _selectedTracks;
        private readonly string _outputMKVPath;

        public FFmpeg(List<string> inputM2TSPaths, List<Track> selectedTracks, string outputMKVPath)
        {
            _ffmpegExePath = Path.Combine(ToolTempDirPath, FFmpegExeFilename);

            _inputM2TSPaths = inputM2TSPaths;
            _selectedTracks = selectedTracks;
            _outputMKVPath = outputMKVPath;

            DoWork += MuxToMKV;
        }

        #region Muxing

        public void MuxToMKV(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            if (_inputM2TSPaths.Count == 0)
                throw new ArgumentOutOfRangeException("At least one input M2TS file is required.");

            var input = _inputM2TSPaths.Count == 1 ? _inputM2TSPaths[0] : "concat:" + string.Join("|", _inputM2TSPaths);
            var args = new ArgumentList();

            args.AddAll("-i", input);

            // Map selected tracks to output file
            args.AddRange(_selectedTracks.SelectMany(track => new[] {"-map", "0:" + track.Index}));

            // Copy all codecs
            args.AddAll("-c", "copy");

            // Convert Blu-ray LPCM tracks to signed, little endian PCM for MKV.
            // Blu-ray LPCM is signed, big endian, and either 16-, 20-, or 24-bit.
            // FFmpeg only outputs 16- or 24- bit PCM, so 20-bit Blu-ray LPCM needs to be converted to 24-bit PCM.
            args.AddRange(_selectedTracks
                .Where(track => track.Codec == MICodec.LPCM)
                .SelectMany(track => new[] { "-c:a:" + track.IndexOfType, "pcm_s" + (track.BitDepth == 16 ? 16 : 24) + "le" }));

            args.Add(_outputMKVPath);

            Execute(args, sender, doWorkEventArgs);
        }

        #endregion

        #region Abstract method implementation

        protected override string Name
        {
            get { return "FFmpeg"; }
        }

        protected override string Filename
        {
            get { return FFmpegExeFilename; }
        }

        protected override void ExtractResources()
        {
            try
            {
                File.WriteAllBytes(_ffmpegExePath, BinTools.ffmpeg);
            }
            catch {}
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override ISet<string> GetOutputFilesImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
