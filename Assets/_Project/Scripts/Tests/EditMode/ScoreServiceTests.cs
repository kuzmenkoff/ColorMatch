using ColorMatch.Core.Config;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class ScoreServiceTests
    {
        private static ScoreService Make(bool clampToZero = false)
        {
            return new ScoreService(new ScoreConfig(pointsPerMatch: 10, penaltyPerMismatch: 5, clampToZero));
        }

        [Test]
        public void ApplyCatch_Match_AddsPoints()
        {
            var score = Make();
            score.ApplyCatch(matched: true);
            Assert.AreEqual(10, score.Current);
        }

        [Test]
        public void ApplyCatch_Mismatch_SubtractsPenalty()
        {
            var score = Make();
            score.ApplyCatch(matched: true);   // 10
            score.ApplyCatch(matched: false);  // 5
            Assert.AreEqual(5, score.Current);
        }

        [Test]
        public void ApplyCatch_Mismatch_WithClamp_DoesNotGoNegative()
        {
            var score = Make(clampToZero: true);
            score.ApplyCatch(matched: false);
            Assert.AreEqual(0, score.Current);
        }

        [Test]
        public void ApplyCatch_Mismatch_WithoutClamp_GoesNegative()
        {
            var score = Make(clampToZero: false);
            score.ApplyCatch(matched: false);
            Assert.AreEqual(-5, score.Current);
        }

        [Test]
        public void ScoreChanged_IsRaised_WithNewValue()
        {
            var score = Make();
            int received = -1;
            score.ScoreChanged += v => received = v;

            score.ApplyCatch(matched: true);

            Assert.AreEqual(10, received);
        }

        [Test]
        public void Reset_SetsScoreToZero()
        {
            var score = Make();
            score.ApplyCatch(matched: true);
            score.Reset();
            Assert.AreEqual(0, score.Current);
        }
    }
}
