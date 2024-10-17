using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moveTxt;
        [SerializeField] private Button _closeBtn;

        private void Awake()
        {
            _closeBtn.onClick.AddListener(CloseUI);
        }

        public void SetMoveTxt(string left, string right, string up, string down)
        {
            _moveTxt.text = string.Concat(left, right, up, down);
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
