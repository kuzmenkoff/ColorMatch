using ColorMatch.Core.Config;
using ColorMatch.Core.Enums;
using ColorMatch.Core.Model;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class SpawnDirectorTests
    {
        private static readonly FigureColor[] Colors =
            { FigureColor.Red, FigureColor.Green, FigureColor.Blue, FigureColor.Yellow };
        private static readonly FigureShape[] Shapes =
            { FigureShape.Circle, FigureShape.Square };

        private static SpawnDirector Make(IRandom random)
        {
            // Constant difficulty: spawn every 1s, fall speed 3.
            var curve = new DifficultyCurve(new DifficultyConfig(
                startFallSpeed: 3f, endFallSpeed: 3f,
                startSpawnInterval: 1f, endSpawnInterval: 1f,
                rampDuration: 0f));
            return new SpawnDirector(curve, Colors, Shapes, random);
        }

        [Test]
        public void Tick_BeforeInterval_DoesNotSpawn()
        {
            var spawner = Make(new QueueRandom());
            int count = 0;
            spawner.FigureRequested += _ => count++;

            spawner.Reset();
            spawner.Tick(0.5f);

            Assert.AreEqual(0, count);
        }

        [Test]
        public void Tick_AtInterval_EmitsRequestWithScriptedValues()
        {
            // color index 2 (Blue), shape index 1 (Square), x = 0.5
            var random = new QueueRandom(ints: new[] { 2, 1 }, floats: new[] { 0.5f });
            var spawner = Make(random);

            FigureSpawnRequest got = default;
            int count = 0;
            spawner.FigureRequested += r => { got = r; count++; };

            spawner.Reset();
            spawner.Tick(0.5f);
            spawner.Tick(0.5f); // reaches 1.0 -> spawn

            Assert.AreEqual(1, count);
            Assert.AreEqual(FigureColor.Blue, got.Color);
            Assert.AreEqual(FigureShape.Square, got.Shape);
            Assert.AreEqual(0.5f, got.NormalizedX, 1e-4f);
            Assert.AreEqual(3f, got.FallSpeed, 1e-4f);
        }
    }
}
