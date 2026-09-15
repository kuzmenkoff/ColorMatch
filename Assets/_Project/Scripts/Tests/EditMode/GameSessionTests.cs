using ColorMatch.Core.Config;
using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class GameSessionTests
    {
        private static readonly FigureColor[] Colors =
            { FigureColor.Red, FigureColor.Green, FigureColor.Blue, FigureColor.Yellow };
        private static readonly FigureShape[] Shapes = { FigureShape.Circle };

        // Builds a session where the basket stays Red and nothing spawns,
        // isolating scoring and the end-of-run flow.
        private static GameSession Make(FakeHighScoreStorage storage, float duration = 2f)
        {
            var random = new QueueRandom(ints: new[] { 0 }); // initial basket color = Red
            var timer = new CountdownTimer(duration);
            var basket = new BasketColorCycler(Colors, interval: 100f, random);
            var curve = new DifficultyCurve(new DifficultyConfig(3f, 3f, 100f, 100f, 0f));
            var spawner = new SpawnDirector(curve, Colors, Shapes, random);
            var score = new ScoreService(new ScoreConfig(10, 5, clampToZero: false));
            return new GameSession(timer, basket, spawner, score, storage);
        }

        [Test]
        public void ReportCatch_Match_AddsPoints_Mismatch_Subtracts()
        {
            var session = Make(new FakeHighScoreStorage());
            session.Start();

            Assert.AreEqual(FigureColor.Red, session.BasketColor);

            session.ReportCatch(FigureColor.Red);  // +10
            Assert.AreEqual(10, session.Score);

            session.ReportCatch(FigureColor.Blue); // -5
            Assert.AreEqual(5, session.Score);
        }

        [Test]
        public void TimerFinished_EndsRun_AndSavesNewHighScore()
        {
            var storage = new FakeHighScoreStorage(initial: 0);
            var session = Make(storage);

            SessionResult result = default;
            int endedCount = 0;
            session.Ended += r => { result = r; endedCount++; };

            session.Start();
            session.ReportCatch(FigureColor.Red); // 10

            session.Tick(1f);
            session.Tick(1.5f); // total 2.5 > 2 -> timer finishes

            Assert.AreEqual(1, endedCount);
            Assert.IsFalse(session.IsRunning);
            Assert.AreEqual(10, result.Score);
            Assert.IsTrue(result.IsNewHighScore);
            Assert.AreEqual(10, storage.Load());
        }

        [Test]
        public void ReportCatch_AfterEnd_IsIgnored()
        {
            var session = Make(new FakeHighScoreStorage());
            session.Start();
            session.ReportCatch(FigureColor.Red); // 10

            session.Tick(3f); // ends the run

            session.ReportCatch(FigureColor.Red); // ignored
            Assert.AreEqual(10, session.Score);
        }
    }
}
