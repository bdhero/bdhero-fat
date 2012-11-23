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
    class TsMuxer : AbstractExternalTool
    {
        private string _outputFilePath;
        private string _basePath;
        private string _metaFilePath;
        private string _chapterTextFilePath;
        private string _chapterXMLFilePath;

        protected override string Name { get { return "TsMuxer"; } }
        protected override string Filename { get { return "tsMuxeR.exe"; } }

        private readonly BDROM _bdrom;
        private readonly TSPlaylistFile _playlist;
        private readonly ICollection<TSStream> _selectedTracks;
        private readonly string _streamClipPaths;
        private readonly string _mplsFileName;
        private readonly string _videoHeight;
        private readonly string _videoWidth;

        public TsMuxer(BDROM bdrom, TSPlaylistFile playlist, ICollection<TSStream> selectedTracks)
        {
            _bdrom = bdrom;
            _playlist = playlist;
            _selectedTracks = selectedTracks;
            _streamClipPaths = GetStreamClipPaths();
            _mplsFileName = Path.GetFileNameWithoutExtension(playlist.FullName);

            if (playlist.VideoStreams.Count > 0)
            {
                var videoTrack = playlist.VideoStreams[0];
                _videoHeight = videoTrack.Height + "";
                // TODO: Width is always 0
                _videoWidth = videoTrack.Width + "";
            }

            DoWork += Mux;
        }

        private string GetStreamClipPaths()
        {
            return string.Join("+", _playlist.StreamClips.Select(GetClipPath));
        }

        private string GetClipPath(TSStreamClip clip)
        {
            return Args.ForCommandLine(Path.Combine(_bdrom.DirectorySTREAM.FullName, clip.DisplayName));
        }

        private static string CodecMetaName(TSStream stream)
        {
            switch (stream.StreamType)
            {
                case TSStreamType.AVC_VIDEO:
                    return "V_MPEG4/ISO/AVC";
                case TSStreamType.VC1_VIDEO:
                    return "V_MS/VFW/WVC1";
                case TSStreamType.MPEG2_VIDEO:
                    return "V_MPEG-2";
                case TSStreamType.AC3_AUDIO:
                case TSStreamType.AC3_PLUS_AUDIO:
                case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                case TSStreamType.AC3_TRUE_HD_AUDIO:
                    return "A_AC3";
                case TSStreamType.DTS_AUDIO:
                case TSStreamType.DTS_HD_AUDIO:
                case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                case TSStreamType.DTS_HD_MASTER_AUDIO:
                    return "A_DTS";
                case TSStreamType.LPCM_AUDIO:
                    return "A_LPCM";
                case TSStreamType.PRESENTATION_GRAPHICS:
                    return "S_HDMV/PGS";
                case TSStreamType.SUBTITLE:
                    return "S_TEXT/UTF8";
                default:
                    return null;
            }
            /*
            V_MPEG4/ISO/AVC - H264
            V_MS/VFW/WVC1 - VC1
            V_MPEG-2 - MPEG2
            A_AC3 - DD (AC3) / DD (E-AC3) / True HD (True HD only tracks with AC3 core inside).
            A_AAC - AAC
            A_DTS - DTS / DTS-HD
            A_MP3 - MPEG audio layer 1/2/3
            A_LPCM - raw pcm data or PCM WAVE file
            S_HDMV/PGS - subtitle format presentation graphic stream.
            S_TEXT/UTF8 - subtitle format SRT. The text file should be in unicode.
            */
        }

        private void Mux(object sender, DoWorkEventArgs e)
        {
            _outputFilePath = (string)e.Argument;

            if (string.IsNullOrEmpty(_outputFilePath))
                throw new ArgumentNullException();

            ExtractResources();

            _basePath = Path.Combine(Path.GetDirectoryName(_outputFilePath), Path.GetFileNameWithoutExtension(_outputFilePath));
            _metaFilePath = _basePath + ".meta.txt";
            _chapterTextFilePath = _basePath + ".chapters.txt";
            _chapterXMLFilePath = _basePath + ".chapters.xml";

            WriteChapterXmlFile(_chapterXMLFilePath);

            WriteMetaFile(_metaFilePath);

            Execute(new List<string> { _metaFilePath, _outputFilePath }, sender, e);
        }
        
        private void WriteChapterXmlFile(string chapterXmlFilePath)
        {
            new ChapterWriter(_playlist).SaveXml(chapterXmlFilePath);
        }

        private void WriteMetaFile(string metaFilePath)
        {
            var lines = new List<string> {"MUXOPT --no-pcr-on-video-pid --new-audio-pes --vbr --vbv-len=500"};

            foreach (var track in _playlist.SortedStreams)
            {
                var line = new List<string> {CodecMetaName(track), _streamClipPaths};

                if (track.IsVideoStream && track.StreamType == TSStreamType.AVC_VIDEO)
                {
                    line.Add("insertSEI");
                    line.Add("contSPS");
                }
                if (track.IsGraphicsStream || track.IsTextStream)
                {
                    line.Add("bottom-offset=24");
                    line.Add("font-border=2");
                    line.Add("text-align=center");
                    line.Add("video-width=" + _videoWidth);
                    line.Add("video-height=" + _videoHeight);
                }
                if (track.IsGraphicsStream || track.IsTextStream || track.IsVideoStream)
                {
                    // If fps is not specified, it is determined from the stream.
                    //line.Add("fps=" + frameRate);
                }
                if (track.LanguageCode != null)
                {
                    line.Add("lang=" + track.LanguageCode);
                }

                line.Add("track=" + track.PID);
                line.Add("mplsFile=" + _mplsFileName);

                var comment = _selectedTracks.Contains(track) ? "" : "#";

                lines.Add(comment + string.Join(", ", line));

                // Video:
                // fps=23.976, insertSEI, contSPS, track=4113, mplsFile=

                // Audio:
                // track=4352, lang=eng, mplsFile=

                // Subtitle:
                // bottom-offset=24,font-border=2,text-align=center,video-width=1920,video-height=1080,fps=23.976, track=4608, lang=eng, mplsFile=
            }

            File.WriteAllLines(metaFilePath, lines);
        }

        protected override void ExtractResources()
        {
            ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            const string regex = @"^(\d+\.\d+)\%";
            const string errorRegex = @"^(?:Can't)";

            if (Regex.IsMatch(line, regex))
            {
                var match = Regex.Match(line, regex);
                Double.TryParse(match.Groups[1].Value, out progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
                isError = true;
                errorMessages.Add(line);
            }
        }

        protected override ISet<string> GetOutputFilesImpl()
        {
            return new HashSet<string>() { _metaFilePath, _chapterTextFilePath, _chapterXMLFilePath, _outputFilePath };
        }
    }
}
