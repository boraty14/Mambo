using UnityEngine;

namespace Game.Scripts
{
    public static class PlayerSave
    {
        private const string HighScoreSave = "HighScoreSave";

        public static int GetHighScore() => PlayerPrefs.GetInt(HighScoreSave, 0);
        public static void SetHighScore(int score) => PlayerPrefs.SetInt(HighScoreSave, score);
    }
}