using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace BDInfo.controllers
{
    class TsMuxer : AbstractExternalTool
    {
        private string exe_path;

        protected override string GetToolName() { return "TsMuxer"; }

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

        protected override void ExtractResources()
        {
            exe_path = this.ExtractResource("tsMuxeR.exe");
        }

        public override void Test()
        {
            MessageBox.Show("\"" + exe_path + "\" exists: " + File.Exists(exe_path));

            ExtractResources();

            MessageBox.Show("\"" + exe_path + "\" exists: " + File.Exists(exe_path));

            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exe_path;
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            MessageBox.Show(output);

            Cleanup();

            MessageBox.Show("\"" + exe_path + "\" exists: " + File.Exists(exe_path));
        }

        public static void StaticTest()
        {
            AbstractExternalTool tool = new TsMuxer();
            tool.Test();
        }
    }
}
