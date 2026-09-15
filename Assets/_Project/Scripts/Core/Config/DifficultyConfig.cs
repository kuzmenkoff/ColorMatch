namespace ColorMatch.Core.Config
{
    /// <summary>
    /// Difficulty tuning. Each parameter interpolates from its "start" value
    /// to its "end" value over <see cref="RampDuration"/> seconds, then holds
    /// at the "end" value. Faster falling and shorter spawn intervals make
    /// the game progressively harder.
    /// </summary>
    public sealed class DifficultyConfig
    {
        public float StartFallSpeed { get; }
        public float EndFallSpeed { get; }
        public float StartSpawnInterval { get; }
        public float EndSpawnInterval { get; }

        /// <summary>Seconds over which difficulty ramps from start to end.</summary>
        public float RampDuration { get; }

        public DifficultyConfig(
            float startFallSpeed, float endFallSpeed,
            float startSpawnInterval, float endSpawnInterval,
            float rampDuration)
        {
            StartFallSpeed = startFallSpeed;
            EndFallSpeed = endFallSpeed;
            StartSpawnInterval = startSpawnInterval;
            EndSpawnInterval = endSpawnInterval;
            RampDuration = rampDuration;
        }
    }
}
