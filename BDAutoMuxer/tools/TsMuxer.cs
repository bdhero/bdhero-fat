using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BDAutoMuxer.BDInfo;
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
        private string _outputFileNameWithoutExtension;
        private string _outputFilePath;
        private string _outputDirPath;
        private string _basePath;
        private string _metaFilePath;
        private string _chapterTextFilePath;
        private string _chapterXmlFilePath;

        protected override string Name { get { return "TsMuxer"; } }
        protected override string Filename { get { return "tsMuxeR.exe"; } }

        private readonly BDInfo.BDROM _bdrom;
        private readonly TSPlaylistFile _playlist;
        private readonly ICollection<TSStream> _selectedTracks;

        private bool IsDemux { get { return _demuxLPCM || _demuxSubtitles; } }
        private readonly bool _demuxLPCM;
        private readonly bool _demuxSubtitles;

        private readonly string _streamClipPaths;
        private readonly string _mplsFileName;
        private readonly string _videoHeight;
        private readonly string _videoWidth;

        /// <summary>
        /// Map of tsMuxeR's demuxed path + filenames to their new human-friendly path + filenames
        /// </summary>
        private readonly Dictionary<string, string> _demuxedFilePaths = new Dictionary<string, string>();

        public TsMuxer(BDInfo.BDROM bdrom, TSPlaylistFile playlist, ICollection<TSStream> selectedTracks, bool demuxLPCM = false, bool demuxSubtitles = false)
        {
            _bdrom = bdrom;
            _playlist = playlist;
            _selectedTracks = selectedTracks;
            
            _demuxLPCM = demuxLPCM;
            _demuxSubtitles = demuxSubtitles;

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
            RunWorkerCompleted += RunWorkerCompletedHandler;
        }

        private void RunWorkerCompletedHandler(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (!IsDemux) return;

            var baseDemuxFilename = GetStreamClipNamesWithoutExtensions();

            foreach (var track in _selectedTracks.Where(ShouldRename))
            {
                AddDemuxedFileIfExists(baseDemuxFilename, track);

                var i = 1;

                while (AddDemuxedFileIfExists(baseDemuxFilename, track, i) && i++ < 100)
                {
                }
            }

            if (!IsSuccess) return;

            // Rename demuxed output files
            foreach (var file in _demuxedFilePaths)
            {
                try
                {
                    // Key = old tsMuxeR filename path
                    // Value = new human-friendly filename path
                    File.Delete(file.Value);
                    File.Move(file.Key, file.Value);
                }
                catch
                {
                }
            }
        }

        private bool AddDemuxedFileIfExists(string baseDemuxFilename, TSStream track, int num = 0)
        {
            var beforeFilename = GetTsMuxerFilename(baseDemuxFilename, track, num);
            var afterFilename = GetTsMuxerFilename(_outputFileNameWithoutExtension, track, num);

            var beforePath = Path.Combine(_outputDirPath, beforeFilename);
            var afterPath = Path.Combine(_outputDirPath, afterFilename);

            var exists = File.Exists(beforePath);

            if (exists)
            {
                _demuxedFilePaths[beforePath] = afterPath;
            }

            return exists;
        }

        private static string GetTsMuxerFilename(string baseDemuxFilename, TSStream track, int num = 0)
        {
            var isSUP = IsSubtitle(track);
            var isLPCM = IsLPCM(track);
            var strNum = num > 0 ? string.Format(".{0}", num) : "";
            var extension = isSUP ? "sup" : isLPCM ? "wav" : "unknown";
            var oldFileName = string.Format("{0}.track_{1}{2}.{3}", baseDemuxFilename, track.PID, strNum, extension);
            return oldFileName;
        }

        private bool ShouldRename(TSStream track)
        {
            return (_demuxSubtitles && IsSubtitle(track)) || (_demuxLPCM && IsLPCM(track));
        }

        private string GetStreamClipNamesWithoutExtensions()
        {
            return string.Join("+", _playlist.StreamClips.Select(GetClipNameWithoutExtension));
        }

        private string GetStreamClipPaths()
        {
            return string.Join("+", _playlist.StreamClips.Select(GetClipPath));
        }

        private string GetClipNameWithoutExtension(TSStreamClip clip)
        {
            return Path.GetFileNameWithoutExtension(clip.Name);
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

            _outputDirPath = Path.GetDirectoryName(_outputFilePath);

            _outputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(_outputFilePath);

            if (_outputDirPath == null || _outputFileNameWithoutExtension == null)
                return;

            _basePath = Path.Combine(_outputDirPath, _outputFileNameWithoutExtension);
            _chapterTextFilePath = _basePath + ".chapters.txt";
            _chapterXmlFilePath = _basePath + ".chapters.xml";

            WriteChapterXmlFile(_chapterXmlFilePath);

            _metaFilePath = WriteMetaFile(_outputFileNameWithoutExtension);

            Execute(new List<string> { _metaFilePath, IsDemux ? _outputDirPath : _outputFilePath }, sender, e);
        }

        private void WriteChapterXmlFile(string chapterXmlFilePath)
        {
            new ChapterWriter(_playlist).SaveXml(chapterXmlFilePath);
        }

        private bool IncludeTrack(TSStream track)
        {
            return !IsDemux || ((_demuxLPCM && IsLPCM(track)) || (_demuxSubtitles && IsSubtitle(track)));
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

        private string WriteMetaFile(string fileNameWithoutExtension)
        {
            var metaFilePath = GetTempPath(fileNameWithoutExtension + string.Format(".{0}.meta.txt", IsDemux ? "demux" : "mux"));

            var lines = new List<string> { string.Format("MUXOPT --no-pcr-on-video-pid --new-audio-pes {0} --vbr --vbv-len=500", IsDemux ? "--demux" : "") };

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

                var comment = _selectedTracks.Where(IncludeTrack).Contains(track) ? "" : "#";

                lines.Add(comment + string.Join(", ", line));

                // Video:
                // fps=23.976, insertSEI, contSPS, track=4113, mplsFile=

                // Audio:
                // track=4352, lang=eng, mplsFile=

                // Subtitle:
                // bottom-offset=24,font-border=2,text-align=center,video-width=1920,video-height=1080,fps=23.976, track=4608, lang=eng, mplsFile=
            }

            File.WriteAllLines(metaFilePath, lines);

            return metaFilePath;
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
                ErrorMessages.Add(line);
            }
        }

        protected override ISet<string> GetOutputFilesImpl()
        {
            var demuxedFiles = new HashSet<string>();
            demuxedFiles.AddRange(_demuxedFilePaths.Keys);
            demuxedFiles.AddRange(_demuxedFilePaths.Values);
            return new HashSet<string>(demuxedFiles) { _metaFilePath, _chapterTextFilePath, _chapterXmlFilePath, _outputFilePath };
        }
    }
}
