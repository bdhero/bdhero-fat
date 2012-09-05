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
