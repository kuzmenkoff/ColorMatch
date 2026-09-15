using ColorMatch.Core.Storage;
using UnityEngine;

namespace ColorMatch.Presentation.Services
{
    /// <summary>High-score persistence backed by PlayerPrefs.</summary>
    public sealed class PlayerPrefsHighScoreStorage : IHighScoreStorage
    {
        private const string Key = "colormatch.highscore";

        public int Load() => PlayerPrefs.GetInt(Key, 0);

        public void Save(int highScore)
        {
            PlayerPrefs.SetInt(Key, highScore);
            PlayerPrefs.Save();
        }
    }
}