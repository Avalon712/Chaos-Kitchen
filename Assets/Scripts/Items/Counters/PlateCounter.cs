using System;
using UnityEngine;

namespace ChaosKitchen.Items.Counters
{
    public sealed class PlateCounter : BaseCounter
    {
        [Range(1, 10)]
        [SerializeField] private int _maxPlateCount = 5;
        [Range(1, 5)]
        [SerializeField] private float _intervalTime = 2;

        private float _timer;

        private void Awake()
        {
            EventManager.Instance.RegisterEvent(GameEvent.GameOver, GameOver);
            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame);
            EventManager.Instance.RegisterEvent(GameEvent.PauseGame, PauseGame);
            EventManager.Instance.RegisterEvent(GameEvent.ContinueGame, ContinueGame);
        }

        private void ContinueGame()
        {
            _isStartGame = true;
        }

        private void PauseGame()
        {
            _isStartGame = false;
        }

        protected override void GameOver()
        {
            base.GameOver();
            for (int i = 0; i < PlacePoint.childCount; i++)
            {
                var obj = PlacePoint.GetChild(PlacePoint.childCount - 1).GetComponent<KitchenObject>();
                KitchenManager.Instance.Put(obj);
            }
        }


        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                //如果当前放有物品则将物品给玩家（前提是玩家没有手持物）
                if (player.HoldKitchenObj == null && PlaceKitchenObject != null)
                {
                    TransferTo(player);
                    AudioManager.Instance.PlayAudio(EventAudio.Pickup);
                }
            }
        }

        private void Update()
        {
            if (_isStartGame && PlacePoint.childCount < _maxPlateCount)
            {
                _timer += Time.deltaTime;
                if (_timer > _intervalTime)
                {
                    AutoGeneratePlate();
                    _timer = 0;
                }
            }
        }

        private void AutoGeneratePlate()
        {
            KitchenObject plate = KitchenManager.Instance.Get(KitchenObjectType.Plate);
            plate.transform.SetParent(PlacePoint);
            plate.transform.SetAsLastSibling();
            plate.transform.localPosition = Vector3.zero + Vector3.up * 0.1f * PlacePoint.childCount;
            //让其总是指向最后的那个盘子
            PlaceKitchenObject = plate;
        }

        /// <summary>
        /// 不允许放置，只允许拿
        /// </summary>
        public override bool Place(KitchenObject kitchenObject)
        {
            return false;
        }

        public override bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            bool r = base.TransferTo(receiver);
            if (r && PlacePoint.childCount > 0)
            {
                PlaceKitchenObject = PlacePoint.GetChild(PlacePoint.childCount - 1).GetComponent<KitchenObject>();
            }
            return r;
        }
    }
}
