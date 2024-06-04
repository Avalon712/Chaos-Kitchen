using ChaosKitchen.Input;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class PauseUI : MonoBehaviour
    {
        [SerializeField] private Button _continueBtn;
        [SerializeField] private Button _endGame;

        private bool _isStartGame;

        private void Awake()
        {
            _continueBtn.onClick.AddListener(ContinueGame);
            _endGame.onClick.AddListener(GameOver);

            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame);

            //暂停游戏
            GameInput.Instance.Config.Player.Pause.performed += (c) => PauseGame();
        }


        private void StartGame()
        {
            _isStartGame = true;
        }

        private void PauseGame()
        {
            if (_isStartGame)
            {
                _isStartGame = false;
                transform.GetChild(0).gameObject.SetActive(true);
                EventManager.Instance.TriggerEvent(GameEvent.PauseGame);
            }
        }

        private void ContinueGame()
        {
            _isStartGame = true;
            transform.GetChild(0).gameObject.SetActive(false);
            EventManager.Instance.TriggerEvent(GameEvent.ContinueGame);
        }

        private void GameOver()
        {
            _isStartGame = false;
            transform.GetChild(0).gameObject.SetActive(false);
            EventManager.Instance.TriggerEvent(GameEvent.GameOver);
        }

    }
}
