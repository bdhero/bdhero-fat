using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetUtils.TaskUtils
{
    internal class ProgressEstimator
    {
        private readonly ProgressSampleState _state;
        private readonly List<ProgressSampleUnit> _samples;
        private readonly ProgressSampleUnit _lastSample;

        public TimeSpan? EstimatedTimeRemaining { get; private set; }

        public ProgressEstimator(IEnumerable<ProgressSampleUnit> samples, ProgressSampleState state)
        {
            _state = state;
            _samples = samples.ToList();
            _lastSample = _samples.LastOrDefault();

            AddRunningSample();
            Calculate();
        }

        private void AddRunningSample()
        {
            if (_lastSample == null) { return; }
            if (_state != ProgressSampleState.Running) { return; }

            var now = DateTime.Now;

            _samples.Add(new ProgressSampleUnit
                         {
                             DateSampled = now,
                             PercentComplete = _lastSample.PercentComplete,
                             Duration = now - _lastSample.DateSampled
                         });
        }

        private void Calculate()
        {
            EstimatedTimeRemaining = null;

            if (_lastSample == null) { return; }

            var percentageDeltas = new List<double>();
            var durationsInTicks = new List<long>();

            for (var i = 1; i < _samples.Count; i++)
            {
                var prevSample = _samples[i - 1];
                var curSample = _samples[i];

                // From 0.0 to 100.0
                var percentDelta = curSample.PercentComplete - prevSample.PercentComplete;

                percentageDeltas.Add(percentDelta);

                durationsInTicks.Add(curSample.Duration.Ticks);
            }

            var avgPercentageDelta = percentageDeltas.Average();
            var avgDurationInTicks = durationsInTicks.Average();

            // PercentageDelta / Duration.Ticks
            var avgVelocity = avgPercentageDelta / avgDurationInTicks;

            var percentageRemaining = 100.0 - _lastSample.PercentComplete;

            TimeSpan? timeRemaining = null;

            if (avgVelocity > 0)
            {
                var timeRemainingInTicks = percentageRemaining / avgVelocity;
                timeRemaining = TimeSpan.FromTicks((long)timeRemainingInTicks);
            }
            else
            {
//                timeRemaining = lastSample
            }

            timeRemaining = TimeSpan.FromSeconds(Math.Ceiling(timeRemaining.Value.TotalSeconds));

            _lastSample.EstimatedTimeRemaining = timeRemaining;

            EstimatedTimeRemaining = timeRemaining;
        }
    }
}
