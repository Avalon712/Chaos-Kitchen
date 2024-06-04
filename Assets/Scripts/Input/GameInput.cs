using UnityEngine;

namespace ChaosKitchen.Input
{
    [DefaultExecutionOrder(-100)]
    public sealed class GameInput : MonoBehaviour
    {
        private static GameInput _instanced;
        private GameInputConfig _config;

        public static GameInput Instance
        {
            get
            {
                if(_instanced == null)
                {
                    _instanced = new GameObject("Game Input").AddComponent<GameInput>();
                }
                return _instanced;
            }
        }

        public GameInputConfig Config => _config;

        private void Awake()
        {
            _config = new GameInputConfig();
            _instanced = this;
        }

        //-----------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 获取玩家移动输入
        /// </summary>
        /// <returns>归一化后的输入</returns>
        public Vector3 GetPlayerMoveInput()
        {
            Vector2 vector = _config.Player.Move.ReadValue<Vector2>();
            //归一化
            return new Vector3(vector.x, 0, vector.y).normalized;
        }


        //-----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            _instanced = null;
        }
    }
}
