namespace ColorMatch.Core.Services
{
    /// <summary>Outcome of a finished session.</summary>
    public readonly struct SessionResult
    {
        public readonly int Score;
        public readonly int HighScore;
        public readonly bool IsNewHighScore;

        public SessionResult(int score, int highScore, bool isNewHighScore)
        {
            Score = score;
            HighScore = highScore;
            IsNewHighScore = isNewHighScore;
        }
    }
}
