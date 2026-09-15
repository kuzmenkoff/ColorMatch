using ColorMatch.Core.Enums;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Single source of truth for whether a caught figure counts as correct.
    /// </summary>
    public static class ColorMatchRules
    {
        public static bool IsMatch(FigureColor basketColor, FigureColor figureColor) => basketColor == figureColor;
    }
}
