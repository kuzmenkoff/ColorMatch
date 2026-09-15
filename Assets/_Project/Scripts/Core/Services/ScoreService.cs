using System;
using ColorMatch.Core.Config;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Tracks the current run's score and applies the outcome of each catch.
    /// </summary>
    public sealed class ScoreService
    {
        private readonly ScoreConfig _config;

        public int Current { get; private set; }

        /// <summary>Raised after the score changes, carrying the new value.</summary>
        public event Action<int> ScoreChanged;

        public ScoreService(ScoreConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>Applies a caught figure: reward on match, penalty on mismatch.</summary>
        public void ApplyCatch(bool matched)
        {
            Current += matched ? _config.PointsPerMatch : -_config.PenaltyPerMismatch;

            if (_config.ClampToZero && Current < 0)
                Current = 0;

            ScoreChanged?.Invoke(Current);
        }

        /// <summary>Resets the score to zero for a new run.</summary>
        public void Reset()
        {
            Current = 0;
            ScoreChanged?.Invoke(Current);
        }
    }
}
