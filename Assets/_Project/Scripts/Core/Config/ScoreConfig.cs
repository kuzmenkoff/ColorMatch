using System;

namespace ColorMatch.Core.Config
{
    /// <summary>
    /// Scoring parameters: reward for a correct catch, penalty for a wrong
    /// one, and whether the score is prevented from going negative.
    /// </summary>
    public sealed class ScoreConfig
    {
        public int PointsPerMatch { get; }
        public int PenaltyPerMismatch { get; }
        public bool ClampToZero { get; }

        public ScoreConfig(int pointsPerMatch, int penaltyPerMismatch, bool clampToZero)
        {
            if (pointsPerMatch < 0) throw new ArgumentOutOfRangeException(nameof(pointsPerMatch));
            if (penaltyPerMismatch < 0) throw new ArgumentOutOfRangeException(nameof(penaltyPerMismatch));

            PointsPerMatch = pointsPerMatch;
            PenaltyPerMismatch = penaltyPerMismatch;
            ClampToZero = clampToZero;
        }
    }
}
