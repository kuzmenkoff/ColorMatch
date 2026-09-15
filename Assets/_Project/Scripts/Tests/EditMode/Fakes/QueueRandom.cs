using System.Collections.Generic;
using ColorMatch.Core.Services;

namespace ColorMatch.Tests
{
    /// <summary>
    /// Deterministic IRandom that returns pre-scripted values in order,
    /// falling back to a safe default once a queue is drained.
    /// </summary>
    public sealed class QueueRandom : IRandom
    {
        private readonly Queue<int> _ints;
        private readonly Queue<float> _floats;

        public QueueRandom(int[] ints = null, float[] floats = null)
        {
            _ints = new Queue<int>(ints ?? new int[0]);
            _floats = new Queue<float>(floats ?? new float[0]);
        }

        public int Range(int minInclusive, int maxExclusive)
            => _ints.Count > 0 ? _ints.Dequeue() : minInclusive;

        public float NextFloat()
            => _floats.Count > 0 ? _floats.Dequeue() : 0f;
    }
}
