using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BDInfo.controllers
{
    abstract class AbstractExternalTool : BackgroundWorker
    {
        IList<string> paths = new List<string>();

        private Process process = null;
        private string strArgs = null;

        /// <summary>
        /// Command line string used to execute the process, including the full path to the EXE and all arguments.
        /// </summary>
        /// <example>"C:\Program Files (x86)\MKVToolNix\mkvmerge.exe" "arg1" "arg 2"</example>
        public string CommandLine { get { return "\"" + FullName.Replace("\"", "\\\"") + "\"" + " " + strArgs; } }

        /// <summary>
        /// Full path to the EXE.
        /// </summary>
        /// <example>C:\Program Files (x86)\MKVToolNix\mkvmerge.exe</example>
        public string FullName { get { return GetTempPath(Filename); } }

        protected abstract void ExtractResources();
        protected abstract void HandleOutputLine(string line, object sender, DoWorkEventArgs e);
        protected abstract string Name { get; }
        protected abstract string Filename { get; }

        public AbstractExternalTool()
            : base()
        { }

        ~AbstractExternalTool()
        {
            Cleanup();
        }

        protected string GetTempDirectory()
        {
            string path = Path.Combine(Path.GetTempPath(), Process.GetCurrentProcess().Id.ToString(), Name);
            DirectoryInfo dir = Directory.CreateDirectory(path);
            return path;
        }

        protected string GetResourceName(string filename)
        {
            return "BDInfo.lib.exe." + filename;
        }

        protected string GetTempPath(string filename)
        {
            return Path.Combine(GetTempDirectory(), filename);
        }

        protected string ExtractResource(string filename)
        {
            string resource = GetResourceName(filename);
            string destPath = GetTempPath(filename);
            ExtractResource(resource, destPath);
            paths.Add(destPath);
            return destPath;
        }

        // extracts [resource] into the the file specified by [destPath]
        protected void ExtractResource(string resource, string destPath)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(destPath, bytes);
        }

        protected void Execute(IList<string> args, object sender, DoWorkEventArgs e)
        {
            ExtractResources();

            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress(0);

            string[] sanitizedArgs = new string[args.Count];

            for (int i = 0; i < args.Count; i++)
            {
                sanitizedArgs[i] = "\"" + (args[i] != null ? args[i].Replace("\"", "\\\"") : "") + "\"";
            }

            // Start the child process.
            process = new Process();
            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = FullName;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = this.strArgs = string.Join(" ", sanitizedArgs);
            process.Start();

            while (!this.CancellationPending && !process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                HandleOutputLine(line, sender, e);
                // do something with line
            }

            if (this.CancellationPending)
            {
                if (!process.HasExited)
                    process.Kill();
                e.Cancel = true;
                return;
            }

            worker.ReportProgress(100);
        }

        protected void Cleanup()
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }

            foreach (string path in paths)
            {
                File.Delete(path);
            }

            DirectoryInfo dir = new DirectoryInfo(GetTempDirectory());

            if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
            {
                //MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is empty.  Deleting...");
                dir.Delete();
            }
            else
            {
                //MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is NOT empty.  Not deleting.");
            }
        }
    }
}
