using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BDAutoMuxer.controllers;

namespace BDAutoMuxer.tools
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
    // ReSharper disable LocalizableElement
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    // ReSharper restore LocalizableElement
    class EAC3To : AbstractExternalTool
    {
        private const int TRACK_OFFSET = 2;

        private string _outputFileNameWithoutExtension;
        private string _outputFilePath;
        private string _outputDirPath;
        private string _basePath;
        private readonly List<string> _outputFilePaths = new List<string>(); 

        protected override string Name { get { return "EAC3To"; } }
        protected override string Filename { get { return @"eac3to\eac3to.exe"; } }

        private readonly TSPlaylistFile _playlist;
        private readonly ICollection<TSStream> _selectedTracks;

        private readonly bool _demuxLPCM;
        private readonly bool _demuxSubtitles;

        public EAC3To(TSPlaylistFile playlist, ICollection<TSStream> selectedTracks, bool demuxLPCM = false, bool demuxSubtitles = false)
        {
            _playlist = playlist;
            _selectedTracks = selectedTracks;

            _demuxLPCM = demuxLPCM;
            _demuxSubtitles = demuxSubtitles;

            DoWork += Demux;
        }

        private void Demux(object sender, DoWorkEventArgs e)
        {
            _outputFilePath = (string)e.Argument;

            if (string.IsNullOrEmpty(_outputFilePath))
                throw new ArgumentNullException();

            ExtractResources();

            _outputDirPath = Path.GetDirectoryName(_outputFilePath);
            _outputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(_outputFilePath);

            if (_outputDirPath == null || _outputFileNameWithoutExtension == null)
                return;

            _basePath = Path.Combine(_outputDirPath, _outputFileNameWithoutExtension);

            var args = new Args { _playlist.FullName, "1)" };

            for (var i = 0; i < _playlist.SortedStreams.Count; i++)
            {
                var track = _playlist.SortedStreams[i];
                
                if (!IncludeTrack(track)) continue;

                var ext = IsLPCM(track) ? "wav" : IsSubtitle(track) ? "sup" : "oops";

                args.Add(string.Format("{0}:", i + TRACK_OFFSET));
                args.Add(string.Format("{0}._{1}_.track_{2}.{3}", _basePath, track.LanguageCode, track.PID, ext));

                _outputFilePaths.Add(args.Last());
            }

            args.Add("-progressnumbers");

            Execute(args, sender, e);
        }

        private bool IncludeTrack(TSStream track)
        {
            return IsSelected(track) && ((_demuxLPCM && IsLPCM(track)) || (_demuxSubtitles && IsSubtitle(track)));
        }

        private bool IsSelected(TSStream track)
        {
            return _selectedTracks.Contains(track);
        }

        private static bool IsLPCM(TSStream track)
        {
            return track.StreamType == TSStreamType.LPCM_AUDIO;
        }

        private static bool IsSubtitle(TSStream track)
        {
            return (track.IsTextStream) ||
                   (track.IsGraphicsStream && track.StreamType == TSStreamType.PRESENTATION_GRAPHICS);
        }

        protected override void ExtractResources()
        {
            ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            const string regex = @"process: (\d+)\%";
            const string errorRegex = @"(fail|abort)"; // TODO: Improve this

            if (Regex.IsMatch(line, regex))
            {
                var match = Regex.Match(line, regex);
                Double.TryParse(match.Groups[1].Value, out progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
                isError = true;
                ErrorMessages.Add(line);
            }
        }

        protected override ISet<string> GetOutputFilesImpl()
        {
            return new HashSet<string>(_outputFilePaths);
        }
    }
}
