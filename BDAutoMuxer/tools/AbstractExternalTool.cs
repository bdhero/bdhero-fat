using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.views;

namespace BDAutoMuxer.tools
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
// ReSharper disable LocalizableElement
// ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
// ReSharper restore RedundantNameQualifier
// ReSharper restore LocalizableElement
    abstract class AbstractExternalTool : BackgroundWorker
    {
        #region Fields (private)

        private Job _job;
        private BackgroundWorker _worker;

        private readonly IList<string> _paths = new List<string>();
        private string _strArgs;
        private Process _process;

        private Timer _timer = new Timer();
        private DateTime _lastTick = DateTime.Now;
        private DateTime _lastEstimate = DateTime.Now;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private TimeSpan _remainingTime = TimeSpan.Zero;

        private bool _onCompleteHandled;

        #endregion

        #region Fields (protected)

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        protected double progress = 0;

        protected IList<string> errorMessages = new List<string>();

        #endregion

        #region Properties (private)

        private bool isStarted
        {
            get { return IsStarted; }
            set
            {
                IsStarted = value;
                if (!value) return;
                _timer.Start();
                UpdateTime();
            }
        }

        private bool isPaused
        {
            get { return IsPaused; }
            set
            {
                IsPaused = value;
                if (value)
                {
                    UpdateTime();
                    _timer.Stop();
                }
                else
                {
                    _lastTick = DateTime.Now;
                    _timer.Start();
                    UpdateTime();
                }
            }
        }

        private bool isCompleted
        {
            get { return IsCompleted; }
            set
            {
                IsCompleted = value;
                if (value)
                {
                    UpdateTime();
                    _timer.Stop();
                }
            }
        }

        private bool isCanceled
        {
            get { return IsCanceled; }
            set
            {
                IsCanceled = value;
                if (value)
                {
                    UpdateTime();
                    _timer.Stop();
                }
            }
        }

        #endregion

        #region Properties (protected)

        protected bool isError
        {
            get { return IsError; }
            set
            {
                IsError = value;
                if (value)
                {
                    UpdateTime();
                    _timer.Stop();
                }
            }
        }

        /// <summary>
        /// Base directory for all temp files used by this application.
        /// </summary>
        protected static string AppTempDirPath { get { return Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), BDAutoMuxerSettings.AssemblyName)).FullName; } }

        /// <summary>
        /// Directory for temp files used by this tool instance.
        /// </summary>
        protected string ToolTempDirPath { get { return Directory.CreateDirectory(Path.Combine(AppTempDirPath, Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture), Name)).FullName; } }

        protected abstract void ExtractResources();
        protected abstract void HandleOutputLine(string line, object sender, DoWorkEventArgs e);
        protected abstract string Name { get; }
        protected abstract string Filename { get; }

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
        public string CommandLine { get { return Args.ForCommandLine(FullName) + " " + _strArgs; } }

        public bool IsStarted { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsCanceled { get; private set; }
        public bool IsError { get; private set; }

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public Double Progress { get { return progress; } }
        public TimeSpan TimeElapsed { get { return _elapsedTime; } }
        public TimeSpan TimeRemaining { get { return _remainingTime; } }

        public string State
        {
            get
            {
                if (IsError) return "error";
                if (IsCanceled) return "canceled";
                if (IsCompleted) return "completed";
                if (IsPaused) return "paused";
                return "";
            }
        }

        public string ErrorMessage { get { return string.Join("\n", errorMessages); } }

        #endregion

        #region Constructor / Destructor

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
            _paths.Add(destPath);
            return destPath;
        }

        // extracts [resource] into the the file specified by [destPath]
        protected void ExtractResource(string resource, string destPath)
        {
            var dir = Path.GetDirectoryName(destPath);
            Directory.CreateDirectory(dir);
            var stream = GetType().Assembly.GetManifestResourceStream(resource);
            var bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(destPath, bytes);
        }

        #endregion

        protected void Execute(IList<string> args, object sender, DoWorkEventArgs e)
        {
            ExtractResources();

            _worker = (BackgroundWorker)sender;
            _worker.ReportProgress(0);

            progress = 0;
            _timer = new Timer {Interval = 1000};
            _timer.Tick += UpdateTime;
            _lastTick = DateTime.Now;
            _lastEstimate = DateTime.Now;
            _elapsedTime = TimeSpan.Zero;
            _remainingTime = TimeSpan.Zero;
            
            IsStarted = false;
            IsPaused = false;
            IsCompleted = false;
            IsCanceled = false;
            IsError = false;

            _onCompleteHandled = false;

            errorMessages.Clear();

            if (_job == null)
                _job = new Job();

            _process = new Process
                           {
                               StartInfo =
                                   {
                                       UseShellExecute = false,
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true,
                                       FileName = FullName,
                                       CreateNoWindow = true,
                                       WindowStyle = ProcessWindowStyle.Hidden,
                                       Arguments = _strArgs = new Args(args).ToString()
                                   }
                           };

            // See http://stackoverflow.com/a/2279346/467582
            _process.Exited += (p, e2) => ProcessExited(p, e);
            _process.EnableRaisingEvents = true;

            _process.Start();

            isStarted = true;

            _job.AddProcess(_process.Handle);

            while (!CancellationPending && !_process.StandardOutput.EndOfStream && !isError)
            {
                HandleOutputLine(_process.StandardOutput.ReadLine(), sender, e);
                UpdateTime();
            }

            while (!CancellationPending && !_process.StandardError.EndOfStream && !isError)
            {
                errorMessages.Add(_process.StandardError.ReadLine());
            }

            OnComplete(e);
        }

        private void UpdateTime(object sender = null, EventArgs e = null)
        {
            if (isStarted && IsBusy && !(isCanceled || isCompleted || isError || isPaused))
            {
                var newMs = _elapsedTime.TotalMilliseconds + (DateTime.Now - _lastTick).TotalMilliseconds;

                _elapsedTime = TimeSpan.FromMilliseconds(newMs);
                _lastTick = DateTime.Now;

                if ((DateTime.Now - _lastEstimate).TotalSeconds >= 1)
                {
                    var p = progress / 100;
                    TimeSpan tmpEst;

                    if (p > 0 && p < 1)
                        tmpEst = new TimeSpan((long)(_elapsedTime.Ticks / p) - _elapsedTime.Ticks);
                    else
                        tmpEst = TimeSpan.Zero;

                    if (progress < .5 || progress > 95 || Math.Abs(tmpEst.TotalMinutes - _remainingTime.TotalMinutes) > 2)
                    {
                        // Update estimate
                        _remainingTime = tmpEst;
                    }
                    else
                    {
                        // Decrement by 1 second
                        _remainingTime = TimeSpan.FromMilliseconds(_remainingTime.TotalMilliseconds - 1000);
                    }
                    _lastEstimate = DateTime.Now;
                }
            }
            _worker.ReportProgress((int)progress);
        }

        private void ProcessExited(object sender, CancelEventArgs e)
        {
            var p = sender as Process;
            
            if (p == null || p.ExitCode != 0)
            {
                // Use underscored field name to avoid crashing BDAutoMuxer by calling UpdateTime() unnecessarily
                IsError = true;

                var extraDetails = p != null ? string.Format(" with exit code {0}", p.ExitCode) : "";
                var errorMessage = string.Format("tsMuxeR terminated unexpectedly{0}.", extraDetails);

                errorMessages.Add(errorMessage);
            }
            
            OnComplete(e);
        }

        private void OnComplete(CancelEventArgs e)
        {
            // Prevent this method from being called twice (which crashes BDAutoMuxer)
            if (_onCompleteHandled)
                return;

            _onCompleteHandled = true;

            if (errorMessages.Count > 0)
            {
                isError = true;

                // TODO: Should we use e.Result instead?
                throw new Exception(ErrorMessage);
            }
            
            if (CancellationPending)
            {
                isCanceled = true;
            }

            if (isCanceled || isError)
            {
                if (!_process.HasExited)
                    _process.Kill();
                e.Cancel = true;
                return;
            }

            isCompleted = true;

            _worker.ReportProgress(100);
        }

        protected void Cleanup()
        {
            try
            {
                if (_process != null && !_process.HasExited)
                    _process.Kill();

                foreach (var path in _paths)
                {
                    File.Delete(path);
                }

                var dir = new DirectoryInfo(ToolTempDirPath);

                while (dir != null && dir.FullName != AppTempDirPath)
                {
                    if (FileUtils.IsEmpty(dir))
                    {
                        dir.Delete();
                    }
                    dir = dir.Parent;
                }

                var appTempDir = new DirectoryInfo(AppTempDirPath);

                foreach (var d in appTempDir.EnumerateDirectories().Where(FileUtils.IsEmpty))
                {
                    d.Delete();
                }

                if (FileUtils.IsEmpty(appTempDir))
                    appTempDir.Delete();
            }
            catch
            {
            }
        }

        #region Suspend / Resume Thread

        private bool CanSuspendResumeProcess
        {
            get
            {
                return _process != null && !_process.HasExited & !CancellationPending;
            }
        }

        public void Pause()
        {
            if (!CanSuspendResumeProcess || IsPaused) return;
            SuspendProcess(_process.Id);
            isPaused = true;
        }

        public void Resume()
        {
            if (!CanSuspendResumeProcess || !IsPaused) return;
            ResumeProcess(_process.Id);
            isPaused = false;
        }

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

        [Flags]
        private enum ThreadAccess
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
        private static void SuspendProcess(int pid)
        {
            var proc = Process.GetProcessById(pid);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                SuspendThread(pOpenThread);
            }
        }

        private static void ResumeProcess(int pid)
        {
            var proc = Process.GetProcessById(pid);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                var pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                ResumeThread(pOpenThread);
            }
        }

// ReSharper restore UnusedMember.Local
// ReSharper restore InconsistentNaming

        #endregion
    }
}
