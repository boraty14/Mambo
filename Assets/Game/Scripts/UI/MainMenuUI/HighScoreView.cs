using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.MainMenuUI
{
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _highScoreText;

        private void OnEnable()
        {
            _highScoreText.text = $"HIGH SCORE: {PlayerSave.GetHighScore()}";
        }
    }
}