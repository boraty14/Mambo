using System;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private GameObject _endMenu;

        private void OnEnable()
        {
            EventBus.OnStartGame += OnStartGame;
            EventBus.OnEndGame += OnEndGame;
            EventBus.OnReturnMainMenu += OnReturnMainMenu;
        }

        private void OnDisable()
        {
            EventBus.OnStartGame -= OnStartGame;
            EventBus.OnEndGame -= OnEndGame;
            EventBus.OnReturnMainMenu -= OnReturnMainMenu;
        }

        private void OnStartGame()
        {
            _mainMenu.SetActive(false);
            _gameUI.SetActive(true);
        }

        private void OnEndGame()
        {
            _gameUI.SetActive(false);
            _endMenu.SetActive(true);
        }

        private void OnReturnMainMenu()
        {
            _endMenu.SetActive(false);
            _mainMenu.SetActive(true);
        }
    }
}