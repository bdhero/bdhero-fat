using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace BDHero.Plugin
{
    /// <summary>
    /// Provides information about the overall state, status, and progress of a plugin.
    /// </summary>
    public class ProgressProvider
    {
        #region Constants

        /// <summary>
        /// Default timer interval between updates in milliseconds.
        /// TODO: Remaining time calculation fails if this value is set to 250 ms.
        /// </summary>
        public const double DefaultIntervalMs = 1000;

        #endregion

        #region Public getters and setters

        /// <summary>
        /// The plugin that reports progress to this provider.
        /// </summary>
        public IPlugin Plugin;

        /// <summary>
        /// Gets or sets the percentage of the provider's work that has been completed, from <code>0.0</code> to <code>100.0</code>.
        /// </summary>
        public double PercentComplete { get; private set; }

        /// <summary>
        /// Gets or sets what the provider is currently doing.  E.G., "Parsing 00800.MPLS", "Querying TMDb", "Muxing to MKV".
        /// </summary>
        public string Status { get; private set; }

        #endregion

        #region Public getters

        /// <summary>
        /// Gets the current state of the process.
        /// </summary>
        public ProgressProviderState State { get; protected set; }

        /// <summary>
        /// Gest the total amount of time spent actively running (i.e., not paused).
        /// This property may be accessed at any time and in any state.
        /// </summary>
        public TimeSpan RunTime { get { return _stopwatch.Elapsed; } }

        /// <summary>
        /// Gets the estimated amount of time remaining before the process completes.
        /// </summary>
        public TimeSpan TimeRemaining { get; protected set; }

        /// <summary>
        /// Gets the last exception that was thrown or <code>null</code> if no exceptions have been thrown.
        /// </summary>
        public Exception Exception { get; protected set; }

        #endregion

        #region Public events

        /// <summary>
        /// Invoked approximately once every second and whenever the <see cref="State"/> or <see cref="PercentComplete"/> changes.
        /// </summary>
        public event ProgressUpdateHandler Updated;

        /// <summary>
        /// Invoked when the process starts.
        /// </summary>
        public event ProgressUpdateHandler Started;

        /// <summary>
        /// Invoked when the user pauses the process.
        /// </summary>
        public event ProgressUpdateHandler Paused;

        /// <summary>
        /// Invoked when the user resumes the process.
        /// </summary>
        public event ProgressUpdateHandler Resumed;

        /// <summary>
        /// Invoked when the user terminates the process.
        /// </summary>
        public event ProgressUpdateHandler Canceled;

        /// <summary>
        /// Invoked when the process terminates due to an error.
        /// </summary>
        public event ProgressUpdateHandler Errored;

        /// <summary>
        /// Invoked only when the process completes successfully with no errors.
        /// </summary>
        public event ProgressUpdateHandler Successful;

        /// <summary>
        /// Invoked when the process finishes with any status (canceled, errored, or successful).
        /// </summary>
        public event ProgressUpdateHandler Completed;

        #endregion

        #region Private members

        /// <summary>
        /// Last time the <see cref="_timer"/> interval elapsed.  Used to calculate <see cref="RunTime"/>.
        /// </summary>
        private DateTime _lastTick;

        private readonly Timer _timer;

        private readonly Stopwatch _stopwatch = new Stopwatch();

        #endregion

        #region Constructors and initialization

        public ProgressProvider()
        {
            _timer = new Timer(DefaultIntervalMs);
            _timer.Elapsed += Tick;
            Reset();
        }

        #endregion

        private void Tick(object sender = null, ElapsedEventArgs elapsedEventArgs = null)
        {
            CalculateTimeRemaining();

            if (Updated != null)
                Updated(this);

            _lastTick = DateTime.Now;
        }

        public void Update(double percentComplete, string status)
        {
            PercentComplete = percentComplete;
            Status = status;

            if (Updated != null)
                Updated(this);
        }

        public void Reset()
        {
            if (State == ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to reset: object is in {0} state", State));
            }

            _timer.Stop();
            _stopwatch.Reset();

            State = ProgressProviderState.Ready;
            TimeRemaining = TimeSpan.Zero;
            PercentComplete = 0.0;
            Exception = null;
        }

        public void Start()
        {
            if (State != ProgressProviderState.Ready)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to start: must be in {0}, but object is in {1} state",
                                  ProgressProviderState.Ready, State));
            }

            _lastTick = DateTime.Now;

            State = ProgressProviderState.Running;
            Tick();

            _timer.Start();
            _stopwatch.Start();

            if (Started != null)
                Started(this);

            if (Updated != null)
                Updated(this);
        }

        public void Resume()
        {
            if (State != ProgressProviderState.Paused)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to resume: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Paused, State));
            }

            _lastTick = DateTime.Now;

            State = ProgressProviderState.Running;
            Tick();

            _timer.Start();
            _stopwatch.Start();

            if (Resumed != null)
                Resumed(this);

            if (Updated != null)
                Updated(this);
        }

        public void Pause()
        {
            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to pause: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();

            State = ProgressProviderState.Paused;
            Tick();

            if (Paused != null)
                Paused(this);

            if (Updated != null)
                Updated(this);
        }

        public void Cancel()
        {
            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to cancel: must be in {0} state, but object is in {1} state",
                                  ProgressProviderState.Running, ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();

            State = ProgressProviderState.Canceled;
            Tick();

            if (Canceled != null)
                Canceled(this);

            if (Completed != null)
                Completed(this);

            if (Updated != null)
                Updated(this);
        }

        public void Error(Exception exception)
        {
            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to switch to {0} state: must be in {1} state, but object is in {2} state",
                                  ProgressProviderState.Error, ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();

            State = ProgressProviderState.Error;
            Exception = exception;
            Tick();

            if (Errored != null)
                Errored(this);

            if (Completed != null)
                Completed(this);

            if (Updated != null)
                Updated(this);
        }

        public void Succeed()
        {
            if (State != ProgressProviderState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to switch to {0} state: must be in {1} state, but object is in {2} state",
                                  ProgressProviderState.Success, ProgressProviderState.Running, State));
            }

            _timer.Stop();
            _stopwatch.Stop();

            State = ProgressProviderState.Success;
            PercentComplete = 100.0;
            TimeRemaining = TimeSpan.Zero;

            if (Successful != null)
                Successful(this);

            if (Completed != null)
                Completed(this);

            if (Updated != null)
                Updated(this);
        }

        private void CalculateTimeRemaining()
        {
            // Thread safety :-)
            var percentComplete = PercentComplete;

            // 0.0 to 1.0
            var pct = percentComplete / 100;

            // Previously calculated estimate from the last tick
            var oldEstimate = TimeRemaining;

            // Fresh estimate (might be jerky)
            var newEstimate = TimeSpan.Zero;

            // The final estimate value that will actually be used
            var finalEstimate = oldEstimate;

            // Make sure progress percentage is within legal bounds (0%, 100%)
            if (pct > 0 && pct < 1)
                newEstimate = new TimeSpan((long)(RunTime.Ticks / pct) - RunTime.Ticks);

            // Make sure the user gets fresh calculations when the process is first started and when it's nearly finished.
            var criticalThreshold = TimeSpan.FromMinutes(1.25);
            var isCriticalPeriod = percentComplete < .5 || percentComplete > 95 || oldEstimate < criticalThreshold || newEstimate < criticalThreshold;

            // If the new estimate differs from the previous estimate by more than 2 minutes, use the new estimate.
            var changeThreshold = pct < 0.2 ? TimeSpan.FromMinutes(2) : TimeSpan.FromSeconds(20);
            var isLargeChange = Math.Abs(newEstimate.TotalSeconds - oldEstimate.TotalSeconds) > changeThreshold.TotalSeconds;

            // If the previous estimate was less than 1 second remaining, use the new estimate.
            var lowPreviousEstimate = oldEstimate.TotalMilliseconds < 1000;

            // Use the new estimate
            if (isCriticalPeriod || isLargeChange || lowPreviousEstimate)
            {
                finalEstimate = newEstimate;
            }
            // Decrement the previous estimate by roughly 1 second
            else
            {
                finalEstimate -= (DateTime.Now - _lastTick);
            }

            // If the estimate dips below 0, set it to 30 seconds
            if (finalEstimate.TotalMilliseconds < 0)
                finalEstimate = TimeSpan.FromSeconds(30);

            // Truncate (remove) milliseconds from estimate
            finalEstimate = TimeSpan.FromSeconds(Math.Floor(finalEstimate.TotalSeconds));

            TimeRemaining = finalEstimate;
        }
    }

    public delegate void ProgressUpdateHandler(ProgressProvider progressProvider);

    /// <summary>
    /// Describes the state of a <see cref="ProgressProvider"/>.
    /// States are mutually exclusive; a <see cref="ProgressProvider"/> can only have one state at a time.
    /// </summary>
    public enum ProgressProviderState
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
        /// Process was canceled manually by the user.
        /// </summary>
        Canceled,

        /// <summary>
        /// Process terminated due to an unrecoverable error.
        /// </summary>
        Error,

        /// <summary>
        /// Process finished running and completed successfully.
        /// </summary>
        Success
    }
}
