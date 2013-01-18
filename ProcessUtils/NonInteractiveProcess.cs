using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessUtils
{
    /// <summary>
    /// Represents a console (CLI) process that runs in the background without any user interaction.
    /// </summary>
    public class NonInteractiveProcess
    {
        private readonly string _exePath;
        private readonly ArgumentList _arguments;

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
        public NonInteractiveProcessState State { get { return _state; } }
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
        /// Event handler for processing individual lines of output from stdout.
        /// </summary>
        public event BackgroundProcessStdOutHandler StdOut;

        /// <summary>
        /// Event handler for processing individual lines of output from stderr.
        /// </summary>
        public event BackgroundProcessStdErrHandler StdErr;

        /// <summary>
        /// Invoked when a NonInteractiveProcess is killed manually, terminates abnormally (aborts or crashes), or completes successfully.
        /// </summary>
        public event BackgroundProcessExitedHandler Exited;

        /// <summary>
        /// Constructs a new NonInteractiveProcess that will run the specified executable and pass it the 
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="arguments"></param>
        public NonInteractiveProcess(string exePath, IEnumerable<string> arguments = null)
        {
            _exePath = exePath;
            _arguments = new ArgumentList(arguments ?? new string[]{});
        }

        #region Start / Kill

        /// <summary>
        /// Starts the NonInteractiveProcess synchronously and blocks (i.e., does not return) until the process exits,
        /// either by completing successfully or terminating unsuccessfully.
        /// </summary>
        public void Start()
        {
            if (_state != NonInteractiveProcessState.Ready)
                throw new InvalidOperationException("NonInteractiveProcess.Start() cannot be called more than once.");

            using (var jobObject = new JobObject())
            {
                using (var process = CreateProcess())
                {
                    process.StartInfo.FileName = _exePath;
                    process.StartInfo.Arguments = _arguments.ToString();
                    process.EnableRaisingEvents = true;
                    process.Exited += ProcessOnExited;

                    process.Start();

                    Id = process.Id;
                    Name = process.ProcessName;
                    _state = NonInteractiveProcessState.Running;

                    jobObject.AddProcess(Id);

                    _stopwatch.Start();

                    while (!process.StandardOutput.EndOfStream)
                    {
                        HandleStdOut(process.StandardOutput.ReadLine());
                    }

                    while (!process.StandardError.EndOfStream)
                    {
                        HandleStdOut(process.StandardError.ReadLine());
                    }

                    process.WaitForExit();

                    ExitCode = process.ExitCode;

                    ProcessOnExited();
                }
            }
        }

        /// <summary>
        /// Manually aborts the process immediately.
        /// </summary>
        public void Kill()
        {
            if (_state != NonInteractiveProcessState.Running &&
                _state != NonInteractiveProcessState.Paused)
                return;

            try
            {
                using (var process = GetProcess())
                {
                    if (process == null) return;
                    _state = NonInteractiveProcessState.Killed;
                    process.Kill();
                }
            }
            catch {}
        }

        #endregion

        #region Output handling (stdout and stderr)

        private void HandleStdOut(string line)
        {
            if (StdOut != null)
                StdOut(line);
        }

        private void HandleStdErr(string line)
        {
            if (StdErr != null)
                StdErr(line);
        }

        #endregion

        #region Exit handling

        /// <summary>
        /// Invoked synchronously by <see cref="Start()"/> after waiting for the process to exit.
        /// </summary>
        private void ProcessOnExited()
        {
            _stopwatch.Stop();

            if (_state != NonInteractiveProcessState.Killed)
                _state = ExitCode == 0 ? NonInteractiveProcessState.Completed : NonInteractiveProcessState.Error;

            if (Exited != null)
                Exited(_state, ExitCode, RunTime);
        }

        /// <summary>
        /// Invoked asynchronously by the <see cref="Process.Exited"/> event.
        /// </summary>
        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            var process = sender as Process;
            if (process == null) return;
        }

        #endregion

        #region Pause / Resume

        public void Pause()
        {
            if (!CanPause) return;
            using (var process = GetProcess())
            {
                if (process == null) return;

                process.Suspend();

                _stopwatch.Stop();
                _state = NonInteractiveProcessState.Paused;
            }
        }

        public void Resume()
        {
            if (!CanResume) return;
            using (var process = GetProcess())
            {
                if (process == null) return;
                
                process.Resume();

                _stopwatch.Start();
                _state = NonInteractiveProcessState.Running;
            }
        }

        private bool CanPause
        {
            get
            {
                using (var process = GetProcess())
                {
                    return _state == NonInteractiveProcessState.Running && process != null;
                }
            }
        }

        private bool CanResume
        {
            get
            {
                using (var process = GetProcess())
                {
                    return _state == NonInteractiveProcessState.Paused && process != null;
                }
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
    public delegate void BackgroundProcessExitedHandler(NonInteractiveProcessState state, int exitCode, TimeSpan runTime);
}
