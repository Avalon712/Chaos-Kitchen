using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.Items
{
    public sealed class StoveCounter : BaseCounter
    {
        [SerializeField] private GameObject _showCooking;
        [SerializeField] private GameObject _sizzlingParticles;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _processImg;
        [SerializeField] private GameObject _warnUI;
        private float _cookTime;
        private float _needCookTime;
        private CookState _cookState;

        private void Start()
        {
            EventManager.Instance.RegisterEvent(GameEvent.PauseGame, PauseGame);
            EventManager.Instance.RegisterEvent(GameEvent.ContinueGame, ContinueGame);
            _canvas.transform.forward = Camera.main.transform.forward;
        }

        private void PauseGame()
        {
            _isStartGame = false;
            if (PlaceKitchenObject != null && _cookState != CookState.Burned)
                //关闭声音
                AudioManager.Instance.StopAudio(EventAudio.Sizzle);
        }

        private void ContinueGame()
        {
            _isStartGame = true;
            if (PlaceKitchenObject != null && _cookState != CookState.Burned)
                //播放声音
                AudioManager.Instance.PlayAudio(EventAudio.Sizzle);
        }

        protected override void GameOver()
        {
            _cookState = CookState.Uncooked;
            TransferTo(KitchenManager.Instance);
            HideCookingEffect();
            _processImg.fillAmount = 0;
            _cookTime = 0;
        }

        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                PlaceOrTake(player);
            }
        }

        private void PlaceOrTake(PlayerController player)
        {
            //如果当前柜台上没有放置物品则将玩家的物品放置在此，同时此物品必须是可以进行烹饪的物品
            KitchenObject hold = player.HoldKitchenObj;
            if (PlaceKitchenObject == null && hold != null && KitchenObjectHelper.CanCook(hold.KitchenObjectType))
            {
                player.TransferTo(this);

                //初始化放入的食物的烹饪状态
                _cookState = KitchenObjectHelper.GetCookState(PlaceKitchenObject.KitchenObjectType);
                _needCookTime = KitchenObjectHelper.GetCookTime(PlaceKitchenObject.KitchenObjectType);

                //显示烹饪效果
                if (_cookState != CookState.Burned) { ShowCookingEffect(); }

                //初始化进度条颜色
                _processImg.color = new Color(0.615f, 1, 0.345f, 1);
            }

            //如果当前放有物品则将物品给玩家（前提是玩家没有手持物）
            else if (hold == null)
            {
                TransferTo(player);
                //关闭烹饪效果
                HideCookingEffect();
            }
        }

        private void Update()
        {
            //烤糊了就不允许再进行烤了
            if (_isStartGame && PlaceKitchenObject != null && _cookState != CookState.Burned)
            {
                Cook(Time.deltaTime);
            }
        }

        private void Cook(float deltaTime)
        {
            _cookTime += deltaTime;
            if (_cookTime > _needCookTime)
            {
                EnterNextCookState();
                //进入下一个状态后改变颜色
                _processImg.color = new Color(1, 0.35f, 0.14f, 1);
            }
            _processImg.fillAmount = _cookTime / _needCookTime;
        }

        private void EnterNextCookState()
        {
            //进入下一个状态
            _cookState = (CookState)((int)_cookState + 1);
            _cookTime = 0;
            _needCookTime = KitchenObjectHelper.GetCookTime(PlaceKitchenObject.KitchenObjectType);

            //更换烹饪食物状态
            KitchenObject before = PlaceKitchenObject;
            //回收旧的物品
            KitchenManager.Instance.Put(before);
            KitchenObjectType next = KitchenObjectHelper.GetNextCookStateKitchenObjectType(before.KitchenObjectType);
            //放置新的物品
            Place(KitchenManager.Instance.Get(next));

            if (_cookState == CookState.Cooked)
            {
                _warnUI.SetActive(true);
            }

            //烤糊了自动"关火"
            if (_cookState == CookState.Burned)
            {
                HideCookingEffect();
            }
        }

        private void ShowCookingEffect()
        {
            SetGameObjectActive(_canvas.gameObject, true);
            SetGameObjectActive(_showCooking, true);
            SetGameObjectActive(_sizzlingParticles, true);
            //播放声音
            AudioManager.Instance.PlayAudio(EventAudio.Sizzle);
        }

        private void HideCookingEffect()
        {
            SetGameObjectActive(_canvas.gameObject, false);
            SetGameObjectActive(_showCooking, false);
            SetGameObjectActive(_sizzlingParticles, false);
            SetGameObjectActive(_warnUI, false);
            //关闭声音
            AudioManager.Instance.StopAudio(EventAudio.Sizzle);
        }

        private void SetGameObjectActive(GameObject obj, bool active)
        {
            if (obj.activeSelf != active)
            {
                obj.SetActive(active);
            }
        }
    }

    public enum CookState
    {
        /// <summary>
        /// 生
        /// </summary>
        Uncooked,
        /// <summary>
        /// 熟
        /// </summary>
        Cooked,
        /// <summary>
        /// 糊
        /// </summary>
        Burned,
    }
}
