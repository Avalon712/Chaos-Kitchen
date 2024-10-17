using System;
using System.Collections.Generic;

namespace ChaosKitchen
{
    public sealed class EventManager
    {
        private static EventManager _instanced;

        private List<ValueTuple<GameEvent, bool, Action>> _events;

        private EventManager() { _events = new(); }

        public static EventManager Instance
        {
            get
            {
                if (_instanced == null) { _instanced = new(); }
                return _instanced;
            }
        }

        public void RegisterEvent(GameEvent gameEvent, Action callback, bool justOneCall = false)
        {
            _events.Add((gameEvent, justOneCall, callback));
        }

        public void TriggerEvent(GameEvent gameEvent)
        {
            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].Item1 == gameEvent)
                {
                    _events[i].Item3.Invoke();
                    if (_events[i].Item2)
                    {
                        _events.RemoveAt(i--);
                    }
                }
            }
        }

        public void UnregisterEvent(GameEvent gameEvent, Action callback)
        {
            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].Item1 == gameEvent && _events[i].Item3 == callback)
                {
                    _events.RemoveAt(i);
                    return;
                }
            }
        }

    }

    public enum GameEvent
    {
        /// <summary>
        /// 暂停游戏
        /// </summary>
        PauseGame,

        /// <summary>
        /// 继续游戏
        /// </summary>
        ContinueGame,

        /// <summary>
        /// 开始游戏
        /// </summary>
        StartGame,

        /// <summary>
        /// 游戏结束
        /// </summary>
        GameOver,

        /// <summary>
        /// 重新开始游戏
        /// </summary>
        RestartGame,

        /// <summary>
        /// 开始重新绑定按钮
        /// </summary>
        RebindKey_Start,

        /// <summary>
        /// 结束重新绑定按钮
        /// </summary>
        RebindKey_End,
    }
}
