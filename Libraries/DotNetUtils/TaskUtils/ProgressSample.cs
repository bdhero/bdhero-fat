using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private readonly ConcurrentQueue<Sample> _samples = new ConcurrentQueue<Sample>();

        private DateTime? _lastSampleTime;

        private int _minSampleSize = 5;
        private int _maxSampleSize = 10;

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

        /// <summary>
        /// Adds a sample to the queue and recalculates the estimated time remaining.
        /// </summary>
        /// <param name="percentComplete">From 0.0 to 100.0.</param>
        public void Add(double percentComplete)
        {
            TimeSpan duration;

            if (_lastSampleTime.HasValue)
            {
                duration = DateTime.Now - _lastSampleTime.Value;
                _lastSampleTime = DateTime.Now;
            }
            else
            {
                duration = TimeSpan.Zero;
                _lastSampleTime = DateTime.Now;
            }

            _samples.Enqueue(new Sample
                {
                    Duration = duration,
                    PercentComplete = percentComplete
                });
        }

        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="MinSampleSize"/> is greater than <see cref="MaxSampleSize"/>.</exception>
        private TimeSpan? Calculate()
        {
            LimitSampleSize();

            if (MinSampleSize > MaxSampleSize)
            {
                throw new ArgumentOutOfRangeException("MaxSampleSize must be greater than or equal to MinSampleSize");
            }

            if (_samples.Count < MinSampleSize)
            {
                return null;
            }

            if (!_lastSampleTime.HasValue)
            {
                return null;
            }

            var samples = _samples.ToList();
            var lastPercentComplete = samples.Last().PercentComplete;
            samples.Add(new Sample
                {
                    Duration = DateTime.Now - _lastSampleTime.Value,
                    PercentComplete = lastPercentComplete
                });

            var percentageDeltas = new List<double>();
            var durationsInTicks = new List<long>();

            for (var i = 1; i < samples.Count; i++)
            {
                var prevSample = samples[i - 1];
                var curSample = samples[i];

                // From 0.0 to 100.0
                var percentDelta = curSample.PercentComplete - prevSample.PercentComplete;

                percentageDeltas.Add(percentDelta);

                var curVelocity = percentDelta / curSample.Duration.Ticks;

                durationsInTicks.Add(curSample.Duration.Ticks);
            }

            var avgPercentageDelta = percentageDeltas.Average();
            var avgDurationInTicks = durationsInTicks.Average();

            // PercentageDelta / Duration.Ticks
            var avgVelocity = avgPercentageDelta / avgDurationInTicks;

            var percentageRemaining = 100.0 - lastPercentComplete;
            var timeRemainingInTicks = percentageRemaining / avgVelocity;
            var timeRemaining = TimeSpan.FromTicks((long)timeRemainingInTicks);

            return timeRemaining;
        }

        private void LimitSampleSize()
        {
            Sample result;
            while (_samples.Count > _maxSampleSize && _samples.TryDequeue(out result))
            {
            }
        }

        private class Sample
        {
            public TimeSpan Duration;

            /// <summary>
            /// From 0.0 to 100.0.
            /// </summary>
            public double PercentComplete;

            public TimeSpan EstimatedTimeRemaining;
        }
    }
}
