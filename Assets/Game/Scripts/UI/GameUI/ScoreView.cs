using Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.GameUI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Scoreboard _scoreboard;
        [SerializeField] private TMP_Text _scoreText;

        private void OnEnable()
        {
            EventBus.OnUpdateScore += OnUpdateScore;
            _scoreText.text = _scoreboard.GetScore().ToString();
        }

        private void OnDisable()
        {
            EventBus.OnUpdateScore -= OnUpdateScore;
        }

        private void OnUpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}