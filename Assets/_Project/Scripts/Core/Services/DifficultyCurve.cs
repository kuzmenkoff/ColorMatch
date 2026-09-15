using ColorMatch.Core.Config;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Maps elapsed play time to difficulty parameters by linearly
    /// interpolating the config from its start to its end values.
    /// </summary>
    public sealed class DifficultyCurve
    {
        private readonly DifficultyConfig _config;

        public DifficultyCurve(DifficultyConfig config)
        {
            _config = config ?? throw new System.ArgumentNullException(nameof(config));
        }

        /// <summary>Evaluates difficulty at the given elapsed time in seconds.</summary>
        public DifficultySnapshot Evaluate(float elapsedSeconds)
        {
            float t = _config.RampDuration <= 0f
                ? 1f
                : Clamp01(elapsedSeconds / _config.RampDuration);

            return new DifficultySnapshot(
                Lerp(_config.StartFallSpeed, _config.EndFallSpeed, t),
                Lerp(_config.StartSpawnInterval, _config.EndSpawnInterval, t));
        }

        // Local math helpers keep the assembly free of any engine dependency.
        private static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
        private static float Lerp(float a, float b, float t) => a + (b - a) * t;
    }
}
