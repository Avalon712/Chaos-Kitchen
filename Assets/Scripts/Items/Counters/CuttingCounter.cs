using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.Items
{
    public sealed class CuttingCounter : BaseCounter
    {
        [SerializeField] private Canvas _canvas;
        [Header("切割进度")]
        [SerializeField] private Image _processImg;
        [Header("切割动画")]
        [SerializeField] private Animator _animator;

        private int _cutCount; //当前玩家切割刀数

        protected override void GameOver()
        {
            base.GameOver();
            TransferTo(KitchenManager.Instance);
            HideUI();
            _cutCount = 0;
        }

        private void Start()
        {
            //和相机的正前方一致
            _canvas.transform.forward = Camera.main.transform.forward;
        }

        public override void Interact(PlayerController player, InteractiveEvent interactiveEvent)
        {
            if (interactiveEvent == InteractiveEvent.PlaceOrTake)
            {
                PlaceOrTake(player);
            }
            else if (interactiveEvent == InteractiveEvent.Cut)
            {
                CutKitchenObject();
            }
        }

        private void PlaceOrTake(PlayerController player)
        {
            //如果当前柜台上没有放置物品则将玩家的物品放置在此
            if (PlaceKitchenObject == null)
            {
                if (player.TransferTo(this))
                {
                    //如果当前放置的物品是一个可以进行切割的物品则显示切割进度
                    if (KitchenObjectHelper.CanCutting(PlaceKitchenObject.KitchenObjectType)) { ShowUI(); }
                    AudioManager.Instance.PlayAudio(EventAudio.Drop);
                }
            }
            //如果当前放有物品则将物品给玩家（前提是玩家没有手持物）
            else if (player.HoldKitchenObj == null)
            {
                //将切割好的物品给玩家同时隐藏UI
                if (TransferTo(player))
                {
                    HideUI();
                    AudioManager.Instance.PlayAudio(EventAudio.Pickup);
                }
            }
        }

        private void CutKitchenObject()
        {
            //判断当前柜台上有物品同时不是切割好的物品才允许切割
            if (PlaceKitchenObject != null && KitchenObjectHelper.CanCutting(PlaceKitchenObject.KitchenObjectType))
            {
                if (SetCutProcess(++_cutCount))
                {
                    KitchenObject before = PlaceKitchenObject;
                    //将之前的物品返回池中
                    KitchenManager.Instance.Put(before);
                    //将当前桌子上的物品替换为当前物品切割好的物品
                    Place(KitchenManager.Instance.Get(KitchenObjectHelper.GetCuttedKitchenObjectType(PlaceKitchenObject.KitchenObjectType)));
                }
            }
        }

        /// <summary>
        /// 设置切割进度
        /// </summary>
        /// <param name="process">当前切割进度</param>
        /// <returns>是否已经切割好了</returns>
        private bool SetCutProcess(int process)
        {
            //播放切割动画
            _animator.SetTrigger("Cut");
            _processImg.fillAmount = ((float)process) / KitchenObjectHelper.GetNeedCuttingCount(PlaceKitchenObject.KitchenObjectType);
            //播放声音
            AudioManager.Instance.PlayAudio(EventAudio.Chop);
            return _processImg.fillAmount >= 1;
        }

        private void ShowUI()
        {
            _canvas.gameObject.SetActive(true);
        }

        private void HideUI()
        {
            _cutCount = 0;
            _processImg.fillAmount = 0;
            _canvas.gameObject.SetActive(false);
        }


    }
}
