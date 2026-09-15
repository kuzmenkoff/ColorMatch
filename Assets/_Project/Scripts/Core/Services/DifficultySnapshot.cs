namespace ColorMatch.Core.Services
{
    /// <summary>Difficulty parameters evaluated at a single moment in time.</summary>
    public readonly struct DifficultySnapshot
    {
        public readonly float FallSpeed;
        public readonly float SpawnInterval;

        public DifficultySnapshot(float fallSpeed, float spawnInterval)
        {
            FallSpeed = fallSpeed;
            SpawnInterval = spawnInterval;
        }
    }
}
