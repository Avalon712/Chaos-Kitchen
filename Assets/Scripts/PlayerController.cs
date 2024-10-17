using ChaosKitchen.Input;
using ChaosKitchen.Items;
using System;
using UnityEngine;

namespace ChaosKitchen
{
    public class PlayerController : MonoBehaviour, ITransfer<KitchenObject>
    {
        private const string WALKING = "IsWalking";

        [Header("�ƶ��ٶ�")]
        [Range(1, 10)]
        [SerializeField] private float _moveSpeed = 4;
        [Header("��ת�ٶ�")]
        [Range(1, 10)]
        [SerializeField] private float _rotationSpeed = 6;
        [Header("��̨����")]
        [SerializeField] private LayerMask _counterLayer;
        [Header("������Ʒ�ĵ�")]
        [SerializeField] private Transform _placePoint;

        //�Ų�����Ҫ��һ��ʱ��������һ��
        private float _playFootStepTimer;

        /// <summary>
        /// ��ǰ����ֳֵ���Ʒ
        /// </summary>
        public KitchenObject HoldKitchenObj { get; private set; }

        private Animator _animator;

        //��һ����ѡ�еĹ�̨
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
            //ȡ����һ��ѡ�еĹ�̨
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
            GameInput.Instance.Config.Player.Enable(); //������ҵ�����

            //����Ʒ�ŵ���̨�ϻ�ӹ�̨��ȡ����Ʒ
            GameInput.Instance.Config.Player.PlaceOrTake.performed +=
                (content) => Interact(InteractiveEvent.PlaceOrTake);

            //����Ʒ�����и�
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
            //��ȡ�ƶ�����
            Vector3 direction = GameInput.Instance.GetPlayerMoveInput();
            //λ���ƶ�
            transform.position += direction * _moveSpeed * Time.deltaTime;
            //����仯������������ֵ����Բ����ʽ���в�ֵ������ƽ����
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * _rotationSpeed);
            //���Ŷ���
            bool isWalking = direction != Vector3.zero;
            _animator.SetBool(WALKING, isWalking);

            _playFootStepTimer += Time.deltaTime;
            //��������
            //�Ų�������Ҫ���ŵ�̫Ƶ��
            if (isWalking && _playFootStepTimer >= 0.13f)
            {
                _playFootStepTimer = 0;
                AudioManager.Instance.PlayAudio(EventAudio.Walk);
            }
        }

        private void SelectCounter(BaseCounter selected)
        {
            //ȡ����һ��ѡ�еĹ�̨
            _lastSelected?.CancelSelect();
            //����Ϊѡ��״̬
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

