using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using NUnit.Framework;

namespace ColorMatch.Tests
{
    public sealed class ColorMatchRulesTests
    {
        [Test]
        public void IsMatch_SameColor_ReturnsTrue()
        {
            Assert.IsTrue(ColorMatchRules.IsMatch(FigureColor.Red, FigureColor.Red));
        }

        [Test]
        public void IsMatch_DifferentColor_ReturnsFalse()
        {
            Assert.IsFalse(ColorMatchRules.IsMatch(FigureColor.Red, FigureColor.Blue));
        }
    }
}
