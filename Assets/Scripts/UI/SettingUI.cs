using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider _volume;
        [SerializeField] private Slider _time;
        [SerializeField] private Button _okBtn;
        [SerializeField] private TimerUI _timerUI;
        [SerializeField] private TMP_Text _timeTxt;

        private void Awake()
        {
            _volume.onValueChanged.AddListener(SetVolume);
            _time.onValueChanged.AddListener(SetTime);
            _okBtn.onClick.AddListener(OK);
        }

        private void SetTime(float value)
        {
            _timerUI.Time = (int)value;
            _timeTxt.text = ((int)value).ToString();
        }

        private void SetVolume(float value)
        {
            AudioManager.Instance.SetVolume(EventAudio.Background, value);
        }

        private void OK()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void OpenUI()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void CloseUI()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
