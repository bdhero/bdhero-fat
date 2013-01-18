using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;

namespace ProcessUtils
{
    /// <summary>
    /// Asynchronous non-interactive background process worker.
    /// </summary>
    public class BackgroundProcessWorker : NonInteractiveProcess
    {
        private readonly Timer _timer = new Timer(1000);

        private readonly BackgroundWorker _worker = new BackgroundWorker
                                                        {
                                                            WorkerReportsProgress = true,
                                                            WorkerSupportsCancellation = true
                                                        };

        /// <summary>
        /// From 0.0 to 100.0.
        /// </summary>
        protected double _progress;

        private DateTime _lastTick = DateTime.Now;

        private TimeSpan _timeRemaining = TimeSpan.Zero;

        /// <summary>
        /// Accessed ONLY from UI thread.
        /// </summary>
        private ProgressState _uiProgressState = new ProgressState();

        /// <summary>
        /// Invoked approximately once every second with the process's current state, progress percentage, and time estimates.
        /// </summary>
        public event BackgroundProgressHandler ProgressUpdated;

        public BackgroundProcessWorker()
        {
            PropertyChanged += OnPropertyChanged;
            _timer.Elapsed += TimerOnTick;
            _worker.DoWork += (sender, args) => Start();
        }

        /// <summary>
        /// Runs in UI and/or BackgroundWorker threads.
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (State == NonInteractiveProcessState.Running)
                _timer.Start();
            else
                _timer.Stop();
        }

        /// <summary>
        /// Runs in UI thread.
        /// </summary>
        private void TimerOnTick(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CalculateTimeRemaining();
            if (ProgressUpdated != null)
                ProgressUpdated(_uiProgressState);
            _lastTick = DateTime.Now;
        }
        
        /// <summary>
        /// Runs in UI thread.
        /// </summary>
        private void CalculateTimeRemaining()
        {
            // 0.0 to 1.0
            var pct = _progress / 100;

            // Previously calculated estimate from the last tick
            var oldEstimate = _timeRemaining;

            // Fresh estimate (might be jerky)
            var newEstimate = TimeSpan.Zero;

            // The final estimate value that will actually be used
            var finalEstimate = oldEstimate;

            // Make sure progress percentage is within legal bounds (0%, 100%)
            if (pct > 0 && pct < 1)
                newEstimate = new TimeSpan((long)(RunTime.Ticks / pct) - RunTime.Ticks);

            // Make sure the user gets fresh calculations when the process is first started and when it's nearly finished.
            var isCriticalPeriod = _progress < .5 || _progress > 95;

            // If the new estimate differs from the previous estimate by more than 2 minutes, use the new estimate.
            var isLargeChange = Math.Abs(newEstimate.TotalMinutes - oldEstimate.TotalMinutes) > 2;

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

            _timeRemaining = finalEstimate;
            _uiProgressState = new ProgressState
                                   {
                                       ProcessState = State,
                                       PercentComplete = _progress,
                                       TimeElapsed = RunTime,
                                       TimeRemaining = newEstimate
                                   };
        }
    }

    public class ProgressState
    {
        /// <summary>
        /// Internal state of the process.
        /// </summary>
        public NonInteractiveProcessState ProcessState = NonInteractiveProcessState.Ready;

        /// <summary>
        /// From 0.0 to 100.0.
        /// </summary>
        public double PercentComplete;

        /// <summary>
        /// Total amount of time the process has been running (excluding any time in which it was paused).
        /// </summary>
        public TimeSpan TimeElapsed = TimeSpan.Zero;

        /// <summary>
        /// Estimated length of time remaining before the process completes.
        /// </summary>
        public TimeSpan TimeRemaining = TimeSpan.Zero;
    }

    public delegate void BackgroundProgressHandler(ProgressState progressState);
}
