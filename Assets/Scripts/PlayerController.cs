using ChaosKitchen.Input;
using ChaosKitchen.Items;
using System;
using UnityEngine;

namespace ChaosKitchen
{
    public class PlayerController : MonoBehaviour, ITransfer<KitchenObject>
    {
        private const string WALKING = "IsWalking";

        [Header("移动速度")]
        [Range(1, 10)]
        [SerializeField] private float _moveSpeed = 4;
        [Header("旋转速度")]
        [Range(1, 10)]
        [SerializeField] private float _rotationSpeed = 6;
        [Header("柜台交互")]
        [SerializeField] private LayerMask _counterLayer;
        [Header("放置物品的点")]
        [SerializeField] private Transform _placePoint;

        //脚步声音要隔一定时间间隔播放一次
        private float _playFootStepTimer;

        /// <summary>
        /// 当前玩家手持的物品
        /// </summary>
        public KitchenObject HoldKitchenObj { get; private set; }

        private Animator _animator;

        //上一个被选中的柜台
        private BaseCounter _lastSelected;

        private bool _isStartGame;

        private void Awake()
        {
            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame);
            EventManager.Instance.RegisterEvent(GameEvent.GameOver, GameOver);
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

        private void StartGame()
        {
            _isStartGame = true;
        }

        private void GameOver()
        {
            _isStartGame = false;
            //取消上一次选中的柜台
            _lastSelected?.CancelSelect();
            _lastSelected = null;
            TransferTo(KitchenManager.Instance);
            _playFootStepTimer = 0;
            _animator.SetBool(WALKING, false);
            transform.position = Vector3.zero;
            transform.forward = Vector3.forward;
        }


        private void Start()
        {
            _animator = transform.GetComponentInChildren<Animator>();
            GameInput.Instance.Config.Player.Enable(); //启用玩家的输入

            //将物品放到柜台上或从柜台上取走物品
            GameInput.Instance.Config.Player.PlaceOrTake.performed +=
                (content) => Interact(InteractiveEvent.PlaceOrTake);

            //将物品进行切割
            GameInput.Instance.Config.Player.Cut.performed +=
                (content) => Interact(InteractiveEvent.Cut);

        }

        private void FixedUpdate()
        {
            if (_isStartGame) { Move(); }
        }

        private void Interact(InteractiveEvent interactiveEvent)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2f, _counterLayer))
            {
                if (hitInfo.transform.TryGetComponent(out BaseCounter counter))
                {
                    SelectCounter(counter);
                    counter.Interact(this, interactiveEvent);
                }
            }
        }

        private void Move()
        {
            //获取移动输入
            Vector3 direction = GameInput.Instance.GetPlayerMoveInput();
            //位置移动
            transform.position += direction * _moveSpeed * Time.deltaTime;
            //朝向变化，对于向量插值采用圆弧方式进行插值（更加平滑）
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * _rotationSpeed);
            //播放动画
            bool isWalking = direction != Vector3.zero;
            _animator.SetBool(WALKING, isWalking);

            _playFootStepTimer += Time.deltaTime;
            //播放声音
            //脚步声音不要播放得太频繁
            if (isWalking && _playFootStepTimer >= 0.13f)
            {
                _playFootStepTimer = 0;
                AudioManager.Instance.PlayAudio(EventAudio.Walk);
            }
        }

        private void SelectCounter(BaseCounter selected)
        {
            //取消上一次选中的柜台
            _lastSelected?.CancelSelect();
            //设置为选中状态
            selected?.Selected();
            _lastSelected = selected;
        }

        public bool TransferTo(ITransfer<KitchenObject> receiver)
        {
            if (HoldKitchenObj != null && receiver.Place(HoldKitchenObj))
            {
                HoldKitchenObj = null;
                return true;
            }
            return false;
        }

        public bool Place(KitchenObject kitchenObject)
        {
            if (HoldKitchenObj == null && kitchenObject != null)
            {
                kitchenObject.transform.SetParent(_placePoint);
                kitchenObject.transform.localPosition = Vector3.zero;
                HoldKitchenObj = kitchenObject;
                return true;
            }
            return false;
        }
    }
}

