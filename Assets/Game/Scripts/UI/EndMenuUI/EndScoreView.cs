using Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.EndMenuUI
{
    public class EndScoreView : MonoBehaviour
    {
        [SerializeField] private GameObject _highScoreText;
        [SerializeField] private TMP_Text _scoreText;

        public void SetGameData(GameData gameData)
        {
            _highScoreText.SetActive(gameData.IsHighScore);
            _scoreText.text = $"SCORE: {gameData.Score}";
        }
    }
}