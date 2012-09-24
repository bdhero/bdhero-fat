using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BDAutoMuxer.controllers;

namespace BDAutoMuxer.tools
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
    [System.ComponentModel.DesignerCategory("Code")]
    abstract class AbstractExternalTool : BackgroundWorker
    {
        #region Fields (private / protected)

        private IList<string> paths = new List<string>();
        private string strArgs = null;
        private Process process = null;

        private bool _isStarted = false;
        private bool _isPaused = false;
        private bool _isCompleted = false;
        private bool _isCanceled = false;
        private bool _isError = false;

        private Timer timer = new Timer();
        private DateTime lastTick = DateTime.Now;
        private DateTime lastEstimate = DateTime.Now;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan remainingTime = TimeSpan.Zero;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        protected double progress = 0;

        protected IList<string> errorMessages = new List<string>();

        #endregion

        #region Properties (private / protected)

        private bool isStarted
        {
            get { return _isStarted; }
            set
            {
                _isStarted = value;
                if (value)
                {
                    timer.Start();
                    UpdateTime();
                }
            }
        }

        private bool isPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (value)
                {
                    UpdateTime();
                    timer.Stop();
                }
                else
                {
                    lastTick = DateTime.Now;
                    timer.Start();
                    UpdateTime();
                }
            }
        }

        private bool isCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                if (value)
                {
                    UpdateTime();
                    timer.Stop();
                }
            }
        }

        private bool isCanceled
        {
            get { return _isCanceled; }
            set
            {
                _isCanceled = value;
                if (value)
                {
                    UpdateTime();
                    timer.Stop();
                }
            }
        }

        protected bool isError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                if (value)
                {
                    UpdateTime();
                    timer.Stop();
                }
            }
        }

        #endregion

        #region Properties (public)

        /// <summary>
        /// Full path to the EXE.
        /// </summary>
        /// <example>C:\Users\Administrator\AppData\Local\Temp\BDAutoRip\584\TsMuxer\tsMuxeR.exe</example>
        public string FullName { get { return GetTempPath(Filename); } }

        /// <summary>
        /// Command line string used to execute the process, including the full path to the EXE and all arguments.
        /// </summary>
        /// <example>"C:\Users\Administrator\AppData\Local\Temp\BDAutoRip\584\TsMuxer\tsMuxeR.exe" "arg1" "arg 2"</example>
        public string CommandLine { get { return "\"" + FullName.Replace("\"", "\\\"") + "\"" + " " + strArgs; } }

        public bool IsStarted { get { return _isStarted; } }
        public bool IsPaused { get { return _isPaused; } }
        public bool IsCompleted { get { return _isCompleted; } }
        public bool IsCanceled { get { return _isCanceled; } }
        public bool IsError { get { return _isError; } }

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public Double Progress { get { return progress; } }
        public TimeSpan TimeElapsed { get { return elapsedTime; } }
        public TimeSpan TimeRemaining { get { return remainingTime; } }

        public string State
        {
            get
            {
                if (this.IsError) return "error";
                if (this.IsCanceled) return "canceled";
                if (this.IsCompleted) return "completed";
                if (this.IsPaused) return "paused";
                return "";
            }
        }

        public string ErrorMessage { get { return string.Join("\n", errorMessages); } }

        #endregion

        #region Properties (protected)

        /// <summary>
        /// Base directory for all temp files used by this application.
        /// </summary>
        protected static string AppTempDirPath { get { return Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), BDAutoMuxerSettings.AssemblyName)).FullName; } }

        /// <summary>
        /// Directory for temp files used by this tool instance.
        /// </summary>
        protected string ToolTempDirPath { get { return Directory.CreateDirectory(Path.Combine(AppTempDirPath, Process.GetCurrentProcess().Id.ToString(), Name)).FullName; } }

        protected abstract void ExtractResources();
        protected abstract void HandleOutputLine(string line, object sender, DoWorkEventArgs e);
        protected abstract string Name { get; }
        protected abstract string Filename { get; }

        #endregion

        #region Constructor / Destructor

        public AbstractExternalTool()
            : base()
        { }

        ~AbstractExternalTool()
        {
            Cleanup();
        }

        #endregion

        #region Resource Paths & Extraction

        protected string GetResourceName(string filename)
        {
            return "BDAutoMuxer.lib.exe." + filename;
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
            string dir = Path.GetDirectoryName(destPath);
            Directory.CreateDirectory(dir);
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(destPath, bytes);
        }

        #endregion

        private BackgroundWorker worker;

        protected void Execute(IList<string> args, object sender, DoWorkEventArgs e, bool skipNullArgs = true)
        {
            ExtractResources();

            worker = sender as BackgroundWorker;
            worker.ReportProgress(0);

            IList<string> sanitizedArgs = new List<string>();

            for (int i = 0; i < args.Count; i++)
            {
                bool skip = args[i] == null && skipNullArgs;
                if (!skip)
                    sanitizedArgs.Add("\"" + (args[i] != null ? args[i].Replace("\"", "\\\"") : "") + "\"");
            }

            progress = 0;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += UpdateTime;
            lastTick = DateTime.Now;
            lastEstimate = DateTime.Now;
            elapsedTime = TimeSpan.Zero;
            remainingTime = TimeSpan.Zero;
            
            _isStarted = false;
            _isPaused = false;
            _isCompleted = false;
            _isCanceled = false;
            _isError = false;

            errorMessages.Clear();

            // Start the child process.
            process = new Process();
            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = FullName;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = this.strArgs = string.Join(" ", sanitizedArgs);
            process.Start();

            isStarted = true;

            while (!this.CancellationPending && !process.StandardOutput.EndOfStream && !this.isError)
            {
                HandleOutputLine(process.StandardOutput.ReadLine(), sender, e);
                UpdateTime();
            }

            while (!this.CancellationPending && !process.StandardError.EndOfStream && !this.isError)
            {
                errorMessages.Add(process.StandardError.ReadLine());
            }

            if (errorMessages.Count > 0)
            {
                isError = true;

                // TODO: Should we use e.Result instead?
                throw new Exception(ErrorMessage);
            }
            else if (this.CancellationPending)
            {
                isCanceled = true;
            }

            if (this.isCanceled || this.isError)
            {
                if (!process.HasExited)
                    process.Kill();
                e.Cancel = true;
                return;
            }

            isCompleted = true;

            worker.ReportProgress(100);
        }

        private void UpdateTime(object sender = null, EventArgs e = null)
        {
            if (this.isStarted && this.IsBusy && !(this.isCanceled || this.isCompleted || this.isError || this.isPaused))
            {
                double newMS = elapsedTime.TotalMilliseconds + (DateTime.Now - lastTick).TotalMilliseconds;
                elapsedTime = TimeSpan.FromMilliseconds(newMS);
                lastTick = DateTime.Now;

                if ((DateTime.Now - lastEstimate).TotalSeconds >= 1)
                {
                    double p = progress / 100;
                    TimeSpan tmpEst;

                    if (p > 0 && p < 1)
                        tmpEst = new TimeSpan((long)((double)elapsedTime.Ticks / p) - elapsedTime.Ticks);
                    else
                        tmpEst = TimeSpan.Zero;

                    if (progress < .5 || progress > 95 || Math.Abs(tmpEst.TotalMinutes - remainingTime.TotalMinutes) > 2)
                    {
                        // Update estimate
                        remainingTime = tmpEst;
                    }
                    else
                    {
                        // Decrement by 1 second
                        remainingTime = TimeSpan.FromMilliseconds(remainingTime.TotalMilliseconds - 1000);
                    }
                    lastEstimate = DateTime.Now;
                }
            }
            worker.ReportProgress((int)progress);
        }

        // TODO: Refactor.  Start at top level temp dir and recursively delete empty dirs
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
                    if (FileUtils.IsEmpty(dir))
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

                if (FileUtils.IsEmpty(appTempDir))
                    appTempDir.Delete();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        #region Suspend / Resume Thread

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

        #endregion
    }
}
