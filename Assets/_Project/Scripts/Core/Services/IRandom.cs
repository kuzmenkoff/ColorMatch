namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Random source abstraction. Keeps the logic deterministic under test
    /// and independent of engine-specific randomness.
    /// </summary>
    public interface IRandom
    {
        /// <summary>Returns an integer in [minInclusive, maxExclusive).</summary>
        int Range(int minInclusive, int maxExclusive);

        /// <summary>Returns a float in [0, 1).</summary>
        float NextFloat();
    }
}
