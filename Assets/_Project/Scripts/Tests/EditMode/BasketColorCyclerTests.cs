using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class BasketColorCyclerTests
    {
        private static readonly FigureColor[] Colors =
            { FigureColor.Red, FigureColor.Green, FigureColor.Blue, FigureColor.Yellow };

        [Test]
        public void Reset_PicksInitialColor()
        {
            var cycler = new BasketColorCycler(Colors, 1f, new QueueRandom(ints: new[] { 0 }));
            cycler.Reset();
            Assert.AreEqual(FigureColor.Red, cycler.CurrentColor);
        }

        [Test]
        public void Tick_BeforeInterval_DoesNotChange()
        {
            var cycler = new BasketColorCycler(Colors, 1f, new QueueRandom(ints: new[] { 0 }));
            cycler.Reset();
            cycler.Tick(0.5f);
            Assert.AreEqual(FigureColor.Red, cycler.CurrentColor);
        }

        [Test]
        public void Tick_AtInterval_ChangesToDifferentColor()
        {
            // Reset roll 0 -> Red. Change roll 0 -> skips current (index 0) -> Green.
            var cycler = new BasketColorCycler(Colors, 1f, new QueueRandom(ints: new[] { 0, 0 }));
            cycler.Reset();      // Red
            cycler.Tick(0.5f);
            cycler.Tick(0.5f);   // -> Green
            Assert.AreEqual(FigureColor.Green, cycler.CurrentColor);
        }

        [Test]
        public void Tick_NeverRepeatsCurrentColor()
        {
            // Two colors, every roll is 0: the skip forces strict alternation.
            var twoColors = new[] { FigureColor.Red, FigureColor.Green };
            var cycler = new BasketColorCycler(twoColors, 1f, new QueueRandom(ints: new[] { 0, 0, 0, 0 }));

            var seen = new System.Collections.Generic.List<FigureColor>();
            cycler.ColorChanged += c => seen.Add(c);

            cycler.Reset();     // Red
            cycler.Tick(1f);    // Green
            cycler.Tick(1f);    // Red
            cycler.Tick(1f);    // Green

            for (int i = 1; i < seen.Count; i++)
                Assert.AreNotEqual(seen[i - 1], seen[i], $"Color repeated at index {i}");
        }

        [Test]
        public void Tick_WhenColorCannotChange_DoesNotRaiseEvent()
        {
            var single = new[] { FigureColor.Red };
            var cycler = new BasketColorCycler(single, 1f, new QueueRandom(ints: new[] { 0 }));

            int changeCount = 0;
            cycler.ColorChanged += _ => changeCount++;

            cycler.Reset();   // fires once with the initial color
            cycler.Tick(1f);  // only one color available -> no change, no event

            Assert.AreEqual(1, changeCount);
        }
    }
}
