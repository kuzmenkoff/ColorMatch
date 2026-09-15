using ColorMatch.Core.Storage;

namespace ColorMatch.Tests
{
    /// <summary>In-memory high-score storage for tests.</summary>
    public sealed class FakeHighScoreStorage : IHighScoreStorage
    {
        private int _value;

        public FakeHighScoreStorage(int initial = 0) => _value = initial;

        public int Load() => _value;
        public void Save(int highScore) => _value = highScore;
    }
}
