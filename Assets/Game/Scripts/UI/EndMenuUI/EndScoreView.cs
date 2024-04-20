using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.EndMenuUI
{
    public class EndScoreView : MonoBehaviour
    {
        [SerializeField] private GameObject _highScoreText;
        [SerializeField] private TMP_Text _scoreText;

        public void ToggleHighScoreText(bool state)
        {
            _highScoreText.SetActive(state);
        }

        public void SetHighScore(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}