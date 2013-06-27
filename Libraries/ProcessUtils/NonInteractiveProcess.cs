using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils;

namespace ProcessUtils
{
    /// <summary>
    /// Represents a console (CLI) process that runs in the background without any user interaction.
    /// </summary>
    public class NonInteractiveProcess : INotifyPropertyChanged
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets the path to the executable.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if ExePath is modified after the process has started.</exception>
        public string ExePath
        {
            get { return _exePath; }
            set
            {
                if (State != NonInteractiveProcessState.Ready)
                    throw new InvalidOperationException("Cannot modify ExePath after process has started.");
                _exePath = value;
            }
        }
        private string _exePath;

        /// <summary>
        /// Gets or sets the list of arguments passed to the executable.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Arguments is modified after the process has started.</exception>
        public ArgumentList Arguments
        {
            get { return _arguments; }
            set
            {
                if (State != NonInteractiveProcessState.Ready)
                    throw new InvalidOperationException("Cannot modify Arguments after process has started.");
                _arguments = value;
            }
        }
        private ArgumentList _arguments = new ArgumentList();

        private Process _process;

        /// <summary>
        /// Gets the system process ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the internal state of the process.
        /// </summary>
        public NonInteractiveProcessState State
        {
            get { return _state; }
            private set
            {
                if (value != _state)
                {
                    Logger.DebugFormat("Process \"{0}\" changing state from {1} to {2}", ExePath, _state, value);
                    _state = value;
                    PropertyChanged.Notify(() => State);
                }
            }
        }
        private NonInteractiveProcessState _state = NonInteractiveProcessState.Ready;

        /// <summary>
        /// Gets the total elapsed run time of the process.
        /// This property may be accessed at any time and in any state.
        /// </summary>
        public TimeSpan RunTime { get { return _stopwatch.Elapsed; } }
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Gets the exit code of the process.
        /// </summary>
        public int ExitCode { get; private set; }

        /// <summary>
        /// Invoked immediately before the process starts running.
        /// </summary>
        public event EventHandler BeforeStart;

        /// <summary>
        /// Event handler for processing individual lines of output from stdout.
        /// </summary>
        public event BackgroundProcessStdOutHandler StdOut;

        /// <summary>
        /// Event handler for processing individual lines of output from stderr.
        /// </summary>
        public event BackgroundProcessStdErrHandler StdErr;

        /// <summary>
        /// Invoked when the process is killed manually, terminates abnormally (aborts or crashes), or completes successfully.
        /// </summary>
        public event BackgroundProcessExitedHandler Exited;

        /// <summary>
        /// Invoked whenever the state of the process changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the last error that occurred (or <code>null</code> if no errors have occurred).
        /// </summary>
        public Exception Exception { get; protected set; }

        #region Start / Kill

        /// <summary>
        /// Starts the NonInteractiveProcess synchronously and blocks (i.e., does not return) until the process exits,
        /// either by completing successfully or terminating unsuccessfully.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public NonInteractiveProcess Start()
        {
            try
            {
                return StartImpl();
            }
            catch (Exception e)
            {
                Logger.Error("Error occurred while starting/running NonInteractiveProcess", e);
                Kill();
                Exception = e;
                State = NonInteractiveProcessState.Error;
                ProcessOnExited();
                throw;
            }
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        private NonInteractiveProcess StartImpl()
        {
            if (State != NonInteractiveProcessState.Ready)
                throw new InvalidOperationException("NonInteractiveProcess.Start() cannot be called more than once.");

            if (!File.Exists(ExePath))
                throw new FileNotFoundException(string.Format("Unable to Start NonInteractiveProcess: File \"{0}\" does not exist", ExePath));

            using (var jobObject = JobObject2.Create())
            {
                using (_process = CreateProcess())
                {
                    _process.StartInfo.FileName = ExePath;
                    _process.StartInfo.Arguments = Arguments.ToString();
                    _process.EnableRaisingEvents = true;
                    _process.Exited += ProcessOnExited;

                    if (BeforeStart != null)
                        BeforeStart(this, EventArgs.Empty);

                    _process.OutputDataReceived += (sender, args) => HandleStdOut(args.Data);
                    _process.ErrorDataReceived += (sender, args) => HandleStdErr(args.Data);

                    Logger.DebugFormat("\"{0}\" {1}", ExePath, Arguments);

                    _process.Start();

                    Id = _process.Id;
                    Name = _process.ProcessName;
                    State = NonInteractiveProcessState.Running;

                    jobObject.AssignProcess(_process);
                    jobObject.SetKillOnClose();

                    _stopwatch.Start();

                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();

                    Logger.DebugFormat("Waiting for process \"{0}\" w/ PID = {1} to exit...", ExePath, Id);

                    _process.WaitForExit();

                    Logger.DebugFormat("Process \"{0}\" w/ PID = {1} exited", ExePath, Id);

                    ExitCode = _process.ExitCode;
                }
            }

            ProcessOnExited();

            return this;
        }

        private bool ShouldKeepRunning
        {
            get
            {
                return State == NonInteractiveProcessState.Running ||
                       State == NonInteractiveProcessState.Paused;
            }
        }

        /// <summary>
        /// Manually aborts the process immediately.
        /// </summary>
        public void Kill()
        {
            if (!CanKill) return;
            Logger.DebugFormat("Killing process \"{0}\" w/ PID = {1}...", ExePath, Id);
            try
            {
                _process.Kill();
            }
            catch (Exception exception)
            {
                Logger.WarnFormat("Unable to kill process \"{0}\": Exception was thrown:\n{1}", ExePath, exception);
            }
            State = NonInteractiveProcessState.Killed;
        }

        private bool CanKill
        {
            get
            {
                var validState = (State == NonInteractiveProcessState.Running ||
                                  State == NonInteractiveProcessState.Paused);
                return validState && _process != null && !_process.HasExited;
            }
        }

        #endregion

        #region Output handling (stdout and stderr)

        private void HandleStdOut(string line)
        {
            if (StdOut != null)
                StdOut(line);
            AfterOutputLineHandled();
        }

        private void HandleStdErr(string line)
        {
            if (StdErr != null)
                StdErr(line);
            AfterOutputLineHandled();
        }

        /// <summary>
        /// Called after StdOut and StdErr are invoked.
        /// Used to calculate progress or do any other work that takes place every time a line of output has been parsed.
        /// </summary>
        protected virtual void AfterOutputLineHandled()
        {
        }

        #endregion

        #region Exit handling

        /// <summary>
        /// Invoked synchronously by <see cref="Start()"/> after waiting for the process to exit.
        /// </summary>
        private void ProcessOnExited()
        {
            Logger.DebugFormat("Process \"{0}\" exited (synchronous event)", ExePath);

            _stopwatch.Stop();

            if (State != NonInteractiveProcessState.Killed)
            {
                var hasError = (State == NonInteractiveProcessState.Error) || ExitCode != 0 || Exception != null;
                State = hasError ? NonInteractiveProcessState.Error : NonInteractiveProcessState.Completed;
            }

            if (Exited != null)
                Exited(State, ExitCode, Exception, RunTime);
        }

        /// <summary>
        /// Invoked asynchronously by the <see cref="Process.Exited"/> event.
        /// </summary>
        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            Logger.DebugFormat("Process \"{0}\" exited (asynchronous event)", ExePath);
            var process = sender as Process;
            if (process == null) return;
        }

        #endregion

        #region Pause / Resume

        public void Pause()
        {
            if (!CanPause) return;

            Logger.DebugFormat("Pausing process \"{0}\"", ExePath);

            _process.Suspend();
            _stopwatch.Stop();
            State = NonInteractiveProcessState.Paused;
        }

        public void Resume()
        {
            if (!CanResume) return;

            Logger.DebugFormat("Resuming process \"{0}\"", ExePath);
                
            _process.Resume();
            _stopwatch.Start();
            State = NonInteractiveProcessState.Running;
        }

        private bool CanPause
        {
            get
            {
                return State == NonInteractiveProcessState.Running && _process != null && !_process.HasExited;
            }
        }

        private bool CanResume
        {
            get
            {
                return State == NonInteractiveProcessState.Paused && _process != null && !_process.HasExited;
            }
        }

        #endregion

        #region Factory methods

        /// <summary>
        /// Attempts to retrieve the Process object for the current process ID.
        /// </summary>
        /// <returns>A Process object of the process is still running; otherwise null if it has already terminated.</returns>
        private Process GetProcess()
        {
            try { return Process.GetProcessById(Id); }
            catch { return null; }
        }

        private static Process CreateProcess()
        {
            return new Process
                       {
                           StartInfo =
                               {
                                   UseShellExecute = false,
                                   RedirectStandardOutput = true,
                                   RedirectStandardError = true,
                                   CreateNoWindow = true,
                                   WindowStyle = ProcessWindowStyle.Hidden,
                               }
                       };
        }

        #endregion
    }

    /// <summary>
    /// Describes the state of a <see cref="NonInteractiveProcess"/>.
    /// States are mutually exclusive; a NonInteractiveProcess can only have one state at a time.
    /// </summary>
    public enum NonInteractiveProcessState
    {
        /// <summary>
        /// Process has not yet started.
        /// </summary>
        Ready,

        /// <summary>
        /// Process has started and is currently running.
        /// </summary>
        Running,

        /// <summary>
        /// Process has started but is paused (suspended).
        /// </summary>
        Paused,

        /// <summary>
        /// Process was killed manually by calling <see cref="NonInteractiveProcess.Kill()"/>.
        /// </summary>
        Killed,

        /// <summary>
        /// Process terminated due to an unrecoverable error.
        /// </summary>
        Error,

        /// <summary>
        /// Process finished running and completed successfully.
        /// </summary>
        Completed
    }

    /// <summary>
    /// Event handler for processing individual lines of output from stdout.
    /// </summary>
    public delegate void BackgroundProcessStdOutHandler(string line);

    /// <summary>
    /// Event handler for processing individual lines of output from stderr.
    /// </summary>
    public delegate void BackgroundProcessStdErrHandler(string line);

    /// <summary>
    /// Event handler that is invoked when a NonInteractiveProcess is killed manually, terminates abnormally (aborts or crashes), or completes successfully.
    /// </summary>
    public delegate void BackgroundProcessExitedHandler(NonInteractiveProcessState state, int exitCode, Exception exception, TimeSpan runTime);

    /// <summary>
    /// Event handler that is invoked whenever the state of a NonInteractiveProcess changes.
    /// </summary>
    public delegate void BackgroundProcessStateChangedHandler(NonInteractiveProcessState state);
}
