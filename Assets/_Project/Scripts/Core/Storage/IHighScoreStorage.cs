namespace ColorMatch.Core.Storage
{
    /// <summary>
    /// Abstraction over high-score persistence.
    /// </summary>
    public interface IHighScoreStorage
    {
        int Load();
        void Save(int highScore);
    }
}
