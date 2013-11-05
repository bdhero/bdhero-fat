using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetUtils.TaskUtils;
using NUnit.Framework;

namespace DotNetUtilsUnitTests.TaskUtils
{
    [TestFixture]
    public class ProgressSampleTest
    {
        [Test]
        public void TestContinuousLinear()
        {
            var sample = new ProgressSample();

            for (var i = 0; i <= 10; i++)
            {
                var percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void TestStalled()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            for (var i = 0; i <= 5; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 6; i <= 10; i++)
            {
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }

        [Test]
        public void TestStallFollowedByResume()
        {
            var sample = new ProgressSample();

            var percentComplete = 0.0;

            for (var i = 0; i <= 5; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 0; i <= 5; i++)
            {
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }

            for (var i = 6; i <= 10; i++)
            {
                percentComplete = 10.0 * i;
                sample.Add(percentComplete);
                Console.WriteLine("{0}%: {1} remaining", percentComplete, sample.EstimatedTimeRemaining);
                Thread.Sleep(1000);
            }
        }
    }
}
