using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BDInfo.controllers
{
    class TsMuxer : AbstractExternalTool
    {
        private string exe_path;

        private string outputFilePath;
        private string basePath;
        private string metaFilePath;
        private string chapterTextFilePath;

        protected override string Name { get { return "TsMuxer"; } }
        protected override string Filename { get { return "tsMuxeR.exe"; } }

        private BDROM BDROM;
        private TSPlaylistFile playlist;
        private ICollection<TSStream> selectedTracks;
        private string streamClipPaths;
        private string mplsFileName;
        private string frameRate = null;
        private string videoHeight = null;
        private string videoWidth = null;
        private double progress = 0;

        public Double Progress { get { return progress; } }

        public TsMuxer(BDROM BDROM, TSPlaylistFile playlist, ICollection<TSStream> selectedTracks)
            : base()
        {
            this.BDROM = BDROM;
            this.playlist = playlist;
            this.selectedTracks = selectedTracks;

            List<string> streamClipPathList = new List<string>();
            foreach (TSStreamClip clip in playlist.StreamClips)
            {
                streamClipPathList.Add("\"" + Path.Combine(BDROM.DirectorySTREAM.FullName, clip.DisplayName.Replace("\"", "\\\"")) + "\"");
            }
            this.streamClipPaths = string.Join("+", streamClipPathList.ToArray());

            this.mplsFileName = Path.GetFileNameWithoutExtension(playlist.FullName);

            if (playlist.VideoStreams.Count > 0)
            {
                TSVideoStream videoTrack = playlist.VideoStreams[0];
                frameRate = videoTrack.FrameRateDescription;
                videoHeight = videoTrack.Height + "";
                videoWidth = videoTrack.Width + "";
            }

            this.DoWork += Mux;
        }

        private string CodecMetaName(TSStream stream)
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
            outputFilePath = e.Argument as string;

            ExtractResources();
            
            basePath = Path.Combine(Path.GetDirectoryName(outputFilePath), Path.GetFileNameWithoutExtension(outputFilePath));
            
            metaFilePath = basePath + ".meta.txt";
            chapterTextFilePath = basePath + ".chapters.txt";

            WriteChapterTextFile(chapterTextFilePath);
            WriteMetaFile(metaFilePath);

            Execute(new List<string>() { metaFilePath, outputFilePath }, sender, e);
        }

        private void WriteChapterTextFile(string chapterTextFilePath)
        {
            new ChapterWriter(playlist).SaveText(chapterTextFilePath);
        }

        private void WriteMetaFile(string metaFilePath)
        {
            List<string> lines = new List<string>();
            
            lines.Add("MUXOPT --no-pcr-on-video-pid --new-audio-pes --vbr --vbv-len=500");

            foreach (TSStream track in playlist.SortedStreams)
            {
                List<string> line = new List<string>();

                line.Add(CodecMetaName(track));
                line.Add(streamClipPaths);

                if (track.IsVideoStream && (track as TSVideoStream).StreamType == TSStreamType.AVC_VIDEO)
                {
                    line.Add("insertSEI");
                    line.Add("contSPS");
                }
                if (track.IsGraphicsStream || track.IsTextStream)
                {
                    line.Add("bottom-offset=24");
                    line.Add("font-border=2");
                    line.Add("text-align=center");
                    line.Add("video-width=" + videoWidth);
                    line.Add("video-height=" + videoHeight);
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
                line.Add("mplsFile=" + mplsFileName);

                string comment = selectedTracks.Contains(track) ? "" : "#";

                lines.Add(comment + string.Join(", ", line));

                // Video:
                // fps=23.976, insertSEI, contSPS, track=4113, mplsFile=

                // Audio:
                // track=4352, lang=eng, mplsFile=

                // Subtitle:
                // bottom-offset=24,font-border=2,text-align=center,video-width=1920,video-height=1080,fps=23.976, track=4608, lang=eng, mplsFile=
            }

            File.WriteAllLines(metaFilePath, lines);

            //Clipboard.SetText(meta);
            //MessageBox.Show(meta);
        }

        protected override void ExtractResources()
        {
            exe_path = this.ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string regex = @"^(\d+\.\d+)\%";
            if (Regex.IsMatch(line, regex))
            {
                Match match = Regex.Match(line, regex);
                Double.TryParse(match.Groups[1].Value, out progress);
                worker.ReportProgress((int)progress);
            }
        }
    }
}
