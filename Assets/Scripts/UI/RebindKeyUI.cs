using UnityEngine;

namespace ChaosKitchen.UI
{
    public sealed class RebindKeyUI : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Instance.RegisterEvent(GameEvent.RebindKey_Start, OpenUI);
            EventManager.Instance.RegisterEvent(GameEvent.RebindKey_End, CloseUI);
        }

        private void OpenUI()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        private void CloseUI()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
