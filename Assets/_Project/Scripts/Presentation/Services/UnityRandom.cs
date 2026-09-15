using ColorMatch.Core.Services;
using UnityEngine;

namespace ColorMatch.Presentation.Services
{
    /// <summary><see cref="IRandom"/> backed by UnityEngine.Random.</summary>
    public sealed class UnityRandom : IRandom
    {
        public int Range(int minInclusive, int maxExclusive)
            => Random.Range(minInclusive, maxExclusive);

        public float NextFloat()
            => Random.value;
    }
}
