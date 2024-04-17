using TMPro;
using UnityEngine;

namespace Game.Scripts.MainMenu
{
    public class HighScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private GameObject _gameObject;


        public void Toggle(bool state)
        {
            _gameObject.SetActive(state);            
        }
    }
}