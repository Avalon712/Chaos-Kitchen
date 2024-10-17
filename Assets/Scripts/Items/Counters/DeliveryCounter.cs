using UnityEngine;

namespace ChaosKitchen.Items.Counters
{
    public sealed class DeliveryCounter : BaseCounter
    {
        [SerializeField] private GameObject _success;
        [SerializeField] private GameObject _fail;

        private void Awake()
        {
            _success.transform.forward = Camera.main.transform.forward;
            _fail.transform.forward = _success.transform.forward;
        }

        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                KitchenObject hold = player.HoldKitchenObj;
                if (hold.KitchenObjectType == KitchenObjectType.Plate)
                {
                    PlateContainer container = hold.gameObject.GetComponent<PlateContainer>();
                    //判断玩家当前的菜是否正确
                    if (OrderManager.Instance.CheckMenu(container.GetFoods()))
                    {
                        container.ClearFoods();
                        //返回池中
                        player.TransferTo(KitchenManager.Instance);

                        //显示成功的UI
                        _success.SetActive(true);
                        Invoke(nameof(HideSuccessUI), 1f);

                        //播放声音
                        AudioManager.Instance.PlayAudio(EventAudio.Success);
                    }
                    else
                    {
                        //显示失败的UI
                        _fail.SetActive(true);
                        Invoke(nameof(HideFailUI), 1f);

                        AudioManager.Instance.PlayAudio(EventAudio.Fail);
                    }
                }
                else
                {
                    AudioManager.Instance.PlayAudio(EventAudio.Warn);
                }
            }
        }

        private void HideSuccessUI()
        {
            _success.SetActive(false);
        }

        private void HideFailUI()
        {
            _fail.SetActive(false);
        }
    }
}
