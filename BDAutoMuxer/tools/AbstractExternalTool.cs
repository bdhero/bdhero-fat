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
using Microsoft.Win32;

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

        protected readonly IList<string> ErrorMessages = new List<string>();

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
            get { return IsSuccess; }
            set
            {
                IsSuccess = value;
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
        public string FullName { get { return GetExePathAbsolute(Filename); } }

        /// <summary>
        /// Command line string used to execute the process, including the full path to the EXE and all arguments.
        /// </summary>
        /// <example>"C:\Users\Administrator\AppData\Local\Temp\BDAutoRip\584\TsMuxer\tsMuxeR.exe" "arg1" "arg 2"</example>
        public string CommandLine { get { return Args.ForCommandLine(FullName) + " " + _strArgs; } }

        public bool IsStarted { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsCanceled { get; private set; }
        public bool IsError { get; private set; }

        /// <summary>
        /// <code>true</code> if this tool finished executing and completed successfully;
        /// <code>false</code> if an error occurred or the process was cancelled.
        /// </summary>
        public bool IsSuccess { get; private set; }

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
                if (IsSuccess) return "completed";
                if (IsPaused) return "paused";
                return "";
            }
        }

        public string ErrorMessage { get { return String.Join("\n", ErrorMessages); } }

        #endregion

        #region Constructor / Destructor

        ~AbstractExternalTool()
        {
            DeleteTempFiles();
        }

        #endregion

        #region Resource Paths & Extraction

        protected string InstallDir
        {
            get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); }
        }

        protected string GetExePathRelative(string exeFilename)
        {
            return Path.Combine("lib", exeFilename);
        }

        protected string GetExePathAbsolute(string exeFilename)
        {
            return Path.Combine(InstallDir, GetExePathRelative(Filename));
        }

        protected string GetTempPath(string filename)
        {
            return Path.Combine(ToolTempDirPath, filename);
        }

        protected string ExtractResource(string filename)
        {
            // TODO: Handle embedded resources like MediaInfo_XML.csv
            return GetExePathAbsolute(filename);
        }

        #endregion

        #region Process Execution

        protected ISet<string> GetAllFilesInOutputDirs()
        {
            var allFiles = new HashSet<string>();
            var outputDirs = new HashSet<string>(GetOutputFilesImpl().Select(Path.GetDirectoryName));
            foreach (var outputDir in outputDirs)
            {
                allFiles.AddRange(new DirectoryInfo(outputDir).GetFiles().Select(fileInfo => fileInfo.FullName));
            }
            return allFiles;
        }

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
            IsSuccess = false;
            IsCanceled = false;
            IsError = false;

            _onCompleteHandled = false;

            ErrorMessages.Clear();

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
                ErrorMessages.Add(_process.StandardError.ReadLine());
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

                    if (progress < .5 || progress > 95 || Math.Abs(tmpEst.TotalMinutes - _remainingTime.TotalMinutes) > 2 || _remainingTime.TotalMilliseconds < 1000)
                    {
                        // Update estimate
                        _remainingTime = tmpEst;
                    }
                    else
                    {
                        // Decrement by 1 second
                        _remainingTime = TimeSpan.FromMilliseconds(_remainingTime.TotalMilliseconds - 1000);
                    }

                    // If the remaining time estimate dips below 0, bump it up by 30 seconds
                    if (_remainingTime.TotalMilliseconds < 0)
                        _remainingTime = TimeSpan.FromMilliseconds((_remainingTime.TotalMilliseconds + 30000));

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

                var extraDetails = p != null ? String.Format(" with exit code {0}", p.ExitCode) : "";
                var errorMessage = String.Format("tsMuxeR terminated unexpectedly{0}.", extraDetails);

                ErrorMessages.Add(errorMessage);
            }
            
            OnComplete(e);
        }

        private void OnComplete(CancelEventArgs e)
        {
            // Prevent this method from being called twice (which crashes BDAutoMuxer)
            if (_onCompleteHandled)
                return;

            _onCompleteHandled = true;

            if (ErrorMessages.Count > 0)
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

        #endregion

        #region Output Files

        protected void DeleteTempFiles()
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

        public ISet<string> GetOutputFiles()
        {
            return new HashSet<string>(GetOutputFilesImpl().Where(File.Exists).Select(Path.GetFullPath));
        }

        protected abstract ISet<string> GetOutputFilesImpl();

        public bool HasOutputFiles()
        {
            return GetOutputFiles().Any();
        }

        public static void DeleteOutputFiles(ICollection<string> outputFiles)
        {
            ISet<string> parents = new HashSet<string>();

            // Delete output files
            foreach (var path in outputFiles.Where(path => !string.IsNullOrWhiteSpace(path) && File.Exists(path)))
            {
                var parent = Path.GetDirectoryName(path);
                if (parent != null)
                    parents.Add(parent);
                DeleteOutputFile(path);
            }

            // Delete empty parent directories
            foreach (var parentDir in parents.Select(parentPath => new DirectoryInfo(parentPath)).Where(parentDir => !parentDir.GetFiles().Any() && !parentDir.GetDirectories().Any()))
            {
                DeleteOutputDir(parentDir);
            }
        }

        public static void DeleteOutputFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }

        protected static void DeleteOutputDir(DirectoryInfo dir)
        {
            try
            {
                dir.Delete();
            }
            catch
            {
            }
        }

        #endregion

        #region EXE Finder

        private const string AppPathsRegKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";

        /// <summary>
        /// <para>Attempts to find the full path to an EXE by searching (in order):</para>
        /// <list type="number">
        ///     <item><description>The HKLM "App Paths" registry key</description></item>
        ///     <item><description>All <code>%PATH%</code> directories</description></item>
        ///     <item><description>If siblingFilename is specified and can be found via 1) or 2), the same directory as siblingFilename</description></item>
        /// </list>
        /// <para></para>
        /// </summary>
        /// <param name="filename">
        /// The name of the EXE file to find (e.g., <code>"cmd.exe"</code>, <code>"mmg.exe"</code>)
        /// </param>
        /// <param name="siblingFilename">
        /// The name of a sibling EXE file that exists in the same directory as <code>filename</code>
        /// and which may or may not be registered with the OS via the HKLM "App Paths" registry key
        /// </param>
        /// <returns>The absolute path to <code>filename</code> if it can be found; otherwise <code>null</code></returns>
        public static string FindExe(string filename, string siblingFilename = null)
        {
            string result;

            // See http://stackoverflow.com/a/909966/467582
            using (
                RegistryKey fileKey =
                    Registry.LocalMachine.OpenSubKey(string.Format(@"{0}\{1}", AppPathsRegKeyPath, filename)))
            {
                if (fileKey != null)
                {
                    result = (string) fileKey.GetValue(string.Empty);
                    fileKey.Close();

                    if (!string.IsNullOrWhiteSpace(result))
                        return result;
                }
            }

            // See http://stackoverflow.com/a/910069/467582
            String pathVar = Environment.GetEnvironmentVariable("path") ?? "";
            String[] folders = pathVar.Split(Path.PathSeparator);
            result = folders.Select(folder => Path.Combine(folder, filename)).FirstOrDefault(File.Exists);

            if (result == null && siblingFilename != null)
            {
                string siblingPath = FindExe(siblingFilename);
                string siblingDir;
                string filePath;

                if ((siblingPath != null) &&
                    (siblingDir = Path.GetDirectoryName(siblingPath)) != null &&
                    File.Exists(filePath = Path.Combine(siblingDir, filename)))
                {
                    result = filePath;
                }
            }

            return result;
        }

        #endregion

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

            if (proc.ProcessName == String.Empty)
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

            if (proc.ProcessName == String.Empty)
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
