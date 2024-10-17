using UnityEngine;

namespace ChaosKitchen.Items
{
    public abstract class BaseCounter : MonoBehaviour, ITransfer<KitchenObject>
    {
        [Header("选中状态")]
        [SerializeField] private GameObject _selected;
        [Header("放置物体的地方")]
        [SerializeField] private Transform _placePoint;

        protected bool _isStartGame;

        private void Awake()
        {
            EventManager.Instance.RegisterEvent(GameEvent.GameOver, GameOver);
            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame);
        }

        protected virtual void StartGame() { _isStartGame = true; }

        protected virtual void GameOver() { _isStartGame = false; }

        /// <summary>
        /// 柜台上放置的物品
        /// </summary>
        public KitchenObject PlaceKitchenObject { get; protected set; }

        public Transform PlacePoint => _placePoint;

        public abstract void Interact(PlayerController player, InteractiveEvent interactiveEvent);

        public virtual bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            if (PlaceKitchenObject != null && receiver.Place(PlaceKitchenObject))
            {
                PlaceKitchenObject = null;
                return true;
            }
            return false;
        }

        public virtual bool Place(KitchenObject kitchenObject)
        {
            if (_placePoint.childCount == 0 && kitchenObject != null)
            {
                kitchenObject.transform.SetParent(_placePoint);
                kitchenObject.transform.localPosition = Vector3.zero;
                PlaceKitchenObject = kitchenObject;
                return true;
            }
            return false;
        }

        public void Selected()
        {
            _selected.SetActive(true);
        }

        public void CancelSelect()
        {
            _selected.SetActive(false);
        }
    }
}
