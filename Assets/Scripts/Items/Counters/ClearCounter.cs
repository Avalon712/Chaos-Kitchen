
namespace ChaosKitchen.Items
{
    public sealed class ClearCounter : BaseCounter
    {
        private PlateContainer _container;

        protected override void GameOver()
        {
            base.GameOver();
            TransferTo(KitchenManager.Instance);
            _container = null;
        }

        /// <summary>
        /// 1. 如果当前柜台上没有放置物品则将玩家的物品放置在此
        /// 2. 如果当前放有物品则将物品给玩家
        /// </summary>
        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                KitchenObject hold = player.HoldKitchenObj;

                //当玩家手里放的是食物时桌上没有餐盘则放到桌上
                if (hold != null && PlaceKitchenObject == null)
                {
                    player.TransferTo(this);

                    //当玩家手里有餐盘同时桌上没有食物时可以将玩家手中的餐盘放到桌子上
                    if (hold.KitchenObjectType == KitchenObjectType.Plate)
                    {
                        _container = PlaceKitchenObject.GetComponent<PlateContainer>();
                    }

                    AudioManager.Instance.PlayAudio(EventAudio.Drop);
                }

                //当玩家手里放的是食物时桌上有餐盘，则将食物放到餐盘里
                else if (_container != null && hold != null && hold.KitchenObjectType != KitchenObjectType.Plate)
                {
                    //检查当前玩家放的食物是否符合食谱
                    _container.AddFood(player.HoldKitchenObj);
                    //将玩家的食物进行回收，因为餐盘容器中已经有了这个食物
                    player.TransferTo(KitchenManager.Instance);

                    AudioManager.Instance.PlayAudio(EventAudio.Drop);
                }

                //当玩家手里有餐盘同时桌上放有食物时可以将桌上的食物放入到餐盘中
                else if (hold != null && hold.KitchenObjectType == KitchenObjectType.Plate && PlaceKitchenObject != null && PlaceKitchenObject.KitchenObjectType != KitchenObjectType.Plate)
                {
                    PlateContainer container = hold.GetComponent<PlateContainer>();
                    TransferTo(container);

                    AudioManager.Instance.PlayAudio(EventAudio.Drop);
                }

                //如果当前放有物品则将物品给玩家（前提是玩家没有手持物）
                else if (player.HoldKitchenObj == null)
                {
                    TransferTo(player);
                    AudioManager.Instance.PlayAudio(EventAudio.Pickup);
                    _container = null;
                }
            }
        }
    }
}
