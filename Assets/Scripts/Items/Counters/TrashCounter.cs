
namespace ChaosKitchen.Items
{
    public sealed class TrashCounter : BaseCounter
    {
        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                //将玩家手中的物品返回对象池
                player.TransferTo(this);
            }
        }

        /// <summary>
        /// 总是返回true
        /// </summary>
        /// <param name="kitchenObject"></param>
        /// <returns></returns>
        public override bool Place(KitchenObject kitchenObject)
        {
            if (kitchenObject != null)
            {
                KitchenManager.Instance.Put(kitchenObject);
                AudioManager.Instance.PlayAudio(EventAudio.Trash);
                return true;
            }
            return false;
        }

        public override bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            return true;
        }
    }
}
