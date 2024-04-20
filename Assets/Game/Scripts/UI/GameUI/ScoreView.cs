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
            _scoreboard.OnScoreUpdate += OnScoreUpdate;
            _scoreText.text = _scoreboard.GetScore().ToString();
        }

        private void OnDisable()
        {
            _scoreboard.OnScoreUpdate -= OnScoreUpdate;
        }

        private void OnScoreUpdate(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}