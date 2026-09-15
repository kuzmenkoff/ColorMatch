using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class CountdownTimerTests
    {
        [Test]
        public void Reset_SetsRemainingToDuration()
        {
            var timer = new CountdownTimer(2f);
            timer.Reset();
            Assert.AreEqual(2f, timer.Remaining, 1e-4f);
            Assert.IsFalse(timer.IsFinished);
        }

        [Test]
        public void Tick_ReducesRemaining()
        {
            var timer = new CountdownTimer(2f);
            timer.Reset();
            timer.Tick(0.5f);
            Assert.AreEqual(1.5f, timer.Remaining, 1e-4f);
        }

        [Test]
        public void Tick_PastZero_ClampsAndRaisesFinishedOnce()
        {
            var timer = new CountdownTimer(2f);
            timer.Reset();

            int finishedCount = 0;
            timer.Finished += () => finishedCount++;

            timer.Tick(2f); // reaches zero
            timer.Tick(1f); // already finished, must not fire again

            Assert.AreEqual(0f, timer.Remaining, 1e-4f);
            Assert.IsTrue(timer.IsFinished);
            Assert.AreEqual(1, finishedCount);
        }
    }
}
