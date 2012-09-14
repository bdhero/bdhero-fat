using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace BDInfo.controllers
{
    // TODO: Add pause support to remaining time estimator
    abstract class AbstractExternalTool : BackgroundWorker
    {
        IList<string> paths = new List<string>();

        private Process process = null;
        private string strArgs = null;

        private DateTime startTime = DateTime.Now;
        private DateTime lastProgressUpdate = DateTime.Now;
        private List<TimeSpan> progressTicks = new List<TimeSpan>();
        private double lastProgress = 0;
        private TimeSpan timeRemaining = TimeSpan.MaxValue;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        protected double progress = 0;
        public Double Progress { get { return progress; } }
        public TimeSpan TimeRemaining { get { return timeRemaining; } }
        public TimeSpan TimeElapsed { get { return DateTime.Now - startTime; } }

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

        /// <summary>
        /// Base directory for all temp files used by this application.
        /// </summary>
        protected static string AppTempDirPath { get { return Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "BDAutoRip")).FullName; } }

        /// <summary>
        /// Directory for temp files used by this tool instance.
        /// </summary>
        protected string ToolTempDirPath { get { return Directory.CreateDirectory(Path.Combine(AppTempDirPath, Process.GetCurrentProcess().Id.ToString(), Name)).FullName; } }

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

        protected string GetResourceName(string filename)
        {
            return "BDInfo.lib.exe." + filename;
        }

        protected string GetTempPath(string filename)
        {
            return Path.Combine(ToolTempDirPath, filename);
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

            progress = 0;
            lastProgress = 0;
            progressTicks.Clear();

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

            startTime = DateTime.Now;
            lastProgressUpdate = DateTime.Now;
            timeRemaining = TimeSpan.Zero;

            while (!this.CancellationPending && !process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                HandleOutputLine(line, sender, e);
                UpdateTime();
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

        private void UpdateTime()
        {
            if (Progress != lastProgress)
            {
                progressTicks.Add(DateTime.Now - lastProgressUpdate);

                lastProgressUpdate = DateTime.Now;
                lastProgress = progress;

                TimeSpan averageSpeed = AverageTimeBetweenTicks();

                if (progressTicks.Count > 20)
                {
                    TimeSpan last5 = AverageTimeBetweenTicks(5);
                    TimeSpan last10 = AverageTimeBetweenTicks(10);
                    TimeSpan last20 = AverageTimeBetweenTicks(20);
                    //timeRemaining = EstimateTimeRemaining((last5.TotalMilliseconds * 0.5) + (last10.TotalMilliseconds * 0.35) + (last20.TotalMilliseconds * 0.15));
                    timeRemaining = EstimateTimeRemaining(last20.TotalMilliseconds, averageSpeed.TotalMilliseconds);
                }
                else if (progressTicks.Count > 10)
                {
                    TimeSpan last5 = AverageTimeBetweenTicks(5);
                    TimeSpan last10 = AverageTimeBetweenTicks(10);
                    //timeRemaining = EstimateTimeRemaining((last5.TotalMilliseconds * 0.75) + (last10.TotalMilliseconds * 0.35));
                    timeRemaining = EstimateTimeRemaining(last10.TotalMilliseconds, averageSpeed.TotalMilliseconds);
                }
                else if (progressTicks.Count > 5)
                {
                    TimeSpan last5 = AverageTimeBetweenTicks(5);
                    //timeRemaining = EstimateTimeRemaining((last5.TotalMilliseconds * 1.0));
                    timeRemaining = EstimateTimeRemaining(last5.TotalMilliseconds, averageSpeed.TotalMilliseconds);
                }
                else
                {
                    timeRemaining = TimeSpan.Zero;
                }
            }
        }

        private static readonly double SMOOTHING_FACTOR = 0.75;

        // TODO: Add pause support to remaining time estimator
        private TimeSpan EstimateTimeRemaining(double lastSpeed, double averageSpeed)
        {
            // averageSpeed = SMOOTHING_FACTOR * lastSpeed + (1-SMOOTHING_FACTOR) * averageSpeed;
            // see http://stackoverflow.com/a/3841706/467582

            double ticksRemaining = 1000.0 - (Progress * 10);
            double newAverage = SMOOTHING_FACTOR * lastSpeed + (1 - SMOOTHING_FACTOR) * averageSpeed;

            TimeSpan newAvg = TimeSpan.FromMilliseconds(newAverage * ticksRemaining);
            return newAvg;
            //return TimeSpan.FromMilliseconds(avgMsPerTick * ticksRemaining);
        }

        private TimeSpan AverageTimeBetweenTicks(int offsetFromLastIndex = -1, int howMany = 0)
        {
            if (offsetFromLastIndex == -1)
                offsetFromLastIndex = progressTicks.Count;

            if (howMany == 0)
                howMany = offsetFromLastIndex;

            // See http://stackoverflow.com/a/1301362/467582
            List<TimeSpan> chunk = progressTicks.Skip(progressTicks.Count - offsetFromLastIndex).Take(howMany).ToList();
            TimeSpan totalTime = TimeSpan.Zero;
            chunk.ForEach((TimeSpan ts) => { totalTime += ts; });
            return TimeSpan.FromMilliseconds(totalTime.TotalMilliseconds / ((double)howMany));
        }

        protected void Cleanup()
        {
            try
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }

                foreach (string path in paths)
                {
                    File.Delete(path);
                }

                DirectoryInfo dir = new DirectoryInfo(ToolTempDirPath);

                while (dir.FullName != AppTempDirPath)
                {
                    if (IsEmpty(dir))
                    {
                        //MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is empty.  Deleting...");
                        dir.Delete();
                    }
                    else
                    {
                        //MessageBox.Show("Temp Directory \"" + GetTempDirectory() + "\" is NOT empty.  Not deleting.");
                    }
                    dir = dir.Parent;
                }

                DirectoryInfo appTempDir = new DirectoryInfo(ToolTempDirPath);

                if (IsEmpty(appTempDir))
                    appTempDir.Delete();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private bool IsEmpty(DirectoryInfo dir)
        {
            return dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0;
        }

        private bool isPaused = false;
        public bool IsPaused { get { return isPaused; } }

        private bool CanSuspendResumeProcess
        {
            get
            {
                return process != null && !process.HasExited & !this.CancellationPending;
            }
        }

        public void Pause()
        {
            if (CanSuspendResumeProcess && !IsPaused)
            {
                SuspendProcess(process.Id);
                isPaused = true;
            }
        }

        public void Resume()
        {
            if (CanSuspendResumeProcess && IsPaused)
            {
                ResumeProcess(process.Id);
                isPaused = false;
            }
        }

        [Flags]
        private enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        /// <see cref="http://stackoverflow.com/questions/71257/suspend-process-in-c-sharp"/>
        private void SuspendProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                SuspendThread(pOpenThread);
            }
        }

        private void ResumeProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                ResumeThread(pOpenThread);
            }
        }
    }
}
