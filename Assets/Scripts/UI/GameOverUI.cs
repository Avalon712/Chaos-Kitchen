using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class GameOverUI : MonoBehaviour
    {
        [SerializeField] private Button _quitBtn;
        [SerializeField] private Button _continueBtn;
        [SerializeField] private TMP_Text _successTxt;

        private void Awake()
        {
            EventManager.Instance.RegisterEvent(GameEvent.GameOver, GameOver);
            _quitBtn.onClick.AddListener(QuitGame);
            _continueBtn.onClick.AddListener(ContinueGame);
        }

        private void GameOver()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _successTxt.text = OrderManager.Instance.SuccessNum.ToString();
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void ContinueGame()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            EventManager.Instance.TriggerEvent(GameEvent.RestartGame);
        }

    }
}
