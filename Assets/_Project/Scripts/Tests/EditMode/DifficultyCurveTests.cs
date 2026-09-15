using ColorMatch.Core.Config;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class DifficultyCurveTests
    {
        private static DifficultyCurve Make()
        {
            // Fall speed 2 -> 6 and interval 1.2 -> 0.4 over 60 seconds.
            return new DifficultyCurve(new DifficultyConfig(
                startFallSpeed: 2f, endFallSpeed: 6f,
                startSpawnInterval: 1.2f, endSpawnInterval: 0.4f,
                rampDuration: 60f));
        }

        [Test]
        public void Evaluate_AtStart_ReturnsStartValues()
        {
            var s = Make().Evaluate(0f);
            Assert.AreEqual(2f, s.FallSpeed, 1e-4f);
            Assert.AreEqual(1.2f, s.SpawnInterval, 1e-4f); 
        }

        [Test]
        public void Evaluate_AtMidpoint_Interpolates()
        {
            var s = Make().Evaluate(30f); // t = 0.5
            Assert.AreEqual(4f, s.FallSpeed, 1e-4f);
            Assert.AreEqual(0.8f, s.SpawnInterval, 1e-4f);
        }

        [Test]
        public void Evaluate_PastRamp_ClampsToEndValues()
        {
            var s = Make().Evaluate(120f);
            Assert.AreEqual(6f, s.FallSpeed, 1e-4f);
            Assert.AreEqual(0.4f, s.SpawnInterval, 1e-4f);
        }
    }
}
