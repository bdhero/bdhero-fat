using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// TODO: Add pause/resume support
    /// </summary>
    public class ProgressSample
    {
        private const int LowestMinSampleSize = 2;

        private readonly ConcurrentQueue<ProgressSampleUnit> _samples = new ConcurrentQueue<ProgressSampleUnit>();

        private int _minSampleSize = 5;
        private int _maxSampleSize = 10;

        public void Reset()
        {
            ProgressSampleUnit result;
            while (_samples.Count > _maxSampleSize && _samples.TryDequeue(out result))
            {
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of samples required to generate a meaningful "time remaining" estimate.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is set to a value less than 2.</exception>
        public int MinSampleSize
        {
            get { return _minSampleSize; }
            set
            {
                if (value < LowestMinSampleSize)
                {
                    throw new ArgumentOutOfRangeException("MinSampleSize cannot be less than " + LowestMinSampleSize);
                }
                _minSampleSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of samples to consider when calculating "time remaining" estimates.
        /// Samples older than this number will be discarded.
        /// </summary>
        public int MaxSampleSize
        {
            get { return _maxSampleSize; }
            set
            {
                _maxSampleSize = value;
            }
        }

        /// <summary>
        /// Gets the estimated time remaining.
        /// </summary>
        public TimeSpan? EstimatedTimeRemaining
        {
            get { return Calculate(); }
        }

        private ProgressSampleState _state;

        private DateTime? LastSampleTime
        {
            get
            {
                var lastSample = _samples.ToArray().LastOrDefault();
                return lastSample != null ? new DateTime?(lastSample.DateSampled) : null;
            }
        }

        private double LastSamplePercent
        {
            get
            {
                var lastSample = _samples.ToArray().LastOrDefault();
                return lastSample != null ? lastSample.PercentComplete : 0.0;
            }
        }

        /// <summary>
        /// Adds a sample to the queue and recalculates the estimated time remaining.
        /// </summary>
        /// <param name="percentComplete">From 0.0 to 100.0.</param>
        public void Add(double percentComplete)
        {
            _state = ProgressSampleState.Running;

            var duration = TimeSpan.Zero;
            var lastSampleTime = LastSampleTime;

            if (lastSampleTime.HasValue)
            {
                duration = DateTime.Now - lastSampleTime.Value;
            }

            Add(percentComplete, duration);
        }

        private void Add(double percentComplete, TimeSpan duration)
        {
            _samples.Enqueue(new ProgressSampleUnit
                             {
                                 DateSampled = DateTime.Now,
                                 Duration = duration,
                                 PercentComplete = percentComplete
                             });
        }

        public void Pause()
        {
            if (_state != ProgressSampleState.Running)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to Pause: must be in {0} state, but instead found {1} state",
                                  ProgressSampleState.Running, _state));
            }

            Add(LastSamplePercent);

            _state = ProgressSampleState.Paused;
        }

        public void Resume()
        {
            if (_state != ProgressSampleState.Paused)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to Resume: must be in {0} state, but instead found {1} state",
                                  ProgressSampleState.Paused, _state));
            }

            Add(LastSamplePercent, TimeSpan.Zero);

            _state = ProgressSampleState.Running;
        }

        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        private bool CanCalculate
        {
            get
            {
                if (MinSampleSize > MaxSampleSize)
                {
                    throw new ArgumentOutOfRangeException("MaxSampleSize must be greater than or equal to MinSampleSize");
                }

                if (_samples.Count < MinSampleSize)
                {
                    return false;
                }

                if (!LastSampleTime.HasValue)
                {
                    return false;
                }

                return true;
            }
        }

        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        private TimeSpan? Calculate()
        {
            LimitSampleSize();

            if (!CanCalculate)
            {
                return null;
            }

            return new ProgressEstimator(_samples, _state).EstimatedTimeRemaining;
        }

        private void LimitSampleSize()
        {
            ProgressSampleUnit result;
            while (_samples.Count > _maxSampleSize && _samples.TryDequeue(out result))
            {
            }
        }
    }
}
