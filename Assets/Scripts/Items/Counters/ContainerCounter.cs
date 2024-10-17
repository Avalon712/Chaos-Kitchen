using UnityEngine;

namespace ChaosKitchen.Items
{
    public sealed class ContainerCounter : BaseCounter
    {
        [Header("当前柜台存储的物品类型")]
        [SerializeField] private KitchenObjectType _storeType;
        [SerializeField] private Animator _animator;

        /// <summary>
        /// 和容器类的柜台交互则为取得相应的类型的物品
        /// </summary>
        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                //判断玩家手里是否有物品，没有则玩家将获得一个此类型的物品
                if (player.HoldKitchenObj == null)
                {
                    _animator.SetTrigger("OpenClose"); //播放动画
                    player.Place(KitchenManager.Instance.Get(_storeType));
                    AudioManager.Instance.PlayAudio(EventAudio.Pickup);
                }
                else
                {
                    Debug.Log("当前玩家手里已经持有物品了，请先放下物品再来拿吧!");
                }
            }
        }

        /// <summary>
        /// 不允传递
        /// </summary>
        /// <returns>总是为false</returns>
        public override bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            return false;
        }

        /// <summary>
        /// 容器类的柜台不允许放置物品
        /// </summary>
        /// <returns>总是为false</returns>
        public override bool Place(KitchenObject kitchenObject)
        {
            return false;
        }
    }
}
