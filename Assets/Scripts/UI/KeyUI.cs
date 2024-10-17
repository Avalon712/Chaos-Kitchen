using ChaosKitchen.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class KeyUI : MonoBehaviour
    {
        [SerializeField]
        private BindingKey _keyType;

        [SerializeField]
        private Button _listenBtn;
        [SerializeField]
        private TMP_Text _keyLabel;

        private void Start()
        {
            _keyLabel.text = GetKeyName();
            _listenBtn.onClick.AddListener(RebindKey);
        }

        private void RebindKey()
        {
            EventManager.Instance.TriggerEvent(GameEvent.RebindKey_Start); //打开UI
            GameInput.Instance.Config.Player.Disable();
            switch (_keyType)
            {
                case BindingKey.Move_Up:
                    GameInput.Instance.Config.Player.Move.PerformInteractiveRebinding(1).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.Move_Down:
                    GameInput.Instance.Config.Player.Move.PerformInteractiveRebinding(2).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.Move_Left:
                    GameInput.Instance.Config.Player.Move.PerformInteractiveRebinding(3).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.Move_Right:
                    GameInput.Instance.Config.Player.Move.PerformInteractiveRebinding(4).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.PlaceOrTake:
                    GameInput.Instance.Config.Player.PlaceOrTake.PerformInteractiveRebinding(0).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.Cut:
                    GameInput.Instance.Config.Player.Cut.PerformInteractiveRebinding(0).OnComplete(OnCompleteRebindKey).Start();
                    break;
                case BindingKey.Pause:
                    GameInput.Instance.Config.Player.Pause.PerformInteractiveRebinding(0).OnComplete(OnCompleteRebindKey).Start();
                    break;
            }
        }

        private void OnCompleteRebindKey(InputActionRebindingExtensions.RebindingOperation operation)
        {
            _keyLabel.text = GetKeyName();
            EventManager.Instance.TriggerEvent(GameEvent.RebindKey_End); //关闭UI
            GameInput.Instance.Config.Player.Enable();
            operation.Dispose();
        }

        private string GetKeyName()
        {
            switch (_keyType)
            {
                //Vector2类型的索引从1开始，其它从索引0开始
                case BindingKey.Move_Up:
                    return GameInput.Instance.Config.Player.Move.bindings[1].ToDisplayString();
                case BindingKey.Move_Down:
                    return GameInput.Instance.Config.Player.Move.bindings[2].ToDisplayString();
                case BindingKey.Move_Left:
                    return GameInput.Instance.Config.Player.Move.bindings[3].ToDisplayString();
                case BindingKey.Move_Right:
                    return GameInput.Instance.Config.Player.Move.bindings[4].ToDisplayString();
                case BindingKey.PlaceOrTake:
                    return GameInput.Instance.Config.Player.PlaceOrTake.bindings[0].ToDisplayString();
                case BindingKey.Cut:
                    return GameInput.Instance.Config.Player.Cut.bindings[0].ToDisplayString();
                case BindingKey.Pause:
                    return GameInput.Instance.Config.Player.Pause.bindings[0].ToDisplayString();
            }
            return null;
        }

    }

    public enum BindingKey
    {
        /// <summary>
        /// 移动键 - 上
        /// </summary>
        Move_Up,

        /// <summary>
        /// 移动键 - 下
        /// </summary>
        Move_Down,

        /// <summary>
        /// 移动键 - 左
        /// </summary>
        Move_Left,

        /// <summary>
        /// 移动键 - 右
        /// </summary>
        Move_Right,

        /// <summary>
        /// 与柜台交互
        /// </summary>
        PlaceOrTake,

        /// <summary>
        /// 切菜
        /// </summary>
        Cut,

        /// <summary>
        /// 游戏暂停
        /// </summary>
        Pause,
    }
}
