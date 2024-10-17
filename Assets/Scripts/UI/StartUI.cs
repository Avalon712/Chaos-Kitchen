using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class StartUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberTxt;
        [SerializeField] private Button _startGameBtn;
        [SerializeField] private Button _tutorialBtn;
        [SerializeField] private Button _settingBtn;

        [SerializeField] private SettingUI _settingUI;
        [SerializeField] private TutorialUI _tutorialUI;

        private int _countTime = 3;

        private void Start()
        {
            _startGameBtn.onClick.AddListener(StartTimer);
            _tutorialBtn.onClick.AddListener(OpenTutorialUI);
            _settingBtn.onClick.AddListener(OpenSettingUI);
            EventManager.Instance.RegisterEvent(GameEvent.RestartGame, RestartGame);
        }

        private void OpenSettingUI()
        {
            _settingUI.OpenUI();
        }

        private void OpenTutorialUI()
        {
            _tutorialUI.OpenUI();
        }

        private void RestartGame()
        {
            gameObject.SetActive(true);
        }

        private void StartTimer()
        {
            _startGameBtn.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(true);
            StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            while (_countTime > 0)
            {
                _numberTxt.text = _countTime.ToString();

                yield return new WaitForSeconds(1f);
                //播放声音
                AudioManager.Instance.PlayAudio(EventAudio.Warn);

                _countTime--;
            }

            _countTime = 3;
            _numberTxt.text = _countTime.ToString();
            EventManager.Instance.TriggerEvent(GameEvent.StartGame);
            gameObject.SetActive(false);
            _startGameBtn.transform.parent.gameObject.SetActive(true);
        }

    }
}
