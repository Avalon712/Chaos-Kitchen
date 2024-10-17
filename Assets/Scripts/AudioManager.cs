using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaosKitchen
{
    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        public class GameAudio
        {
            public EventAudio audioType;
            public AudioClip clip;
            [Range(0, 1)]
            public float volume = 1;
            public bool justOne;
        }

        [SerializeField] private List<GameAudio> _audios;

        private ValueTuple<bool, AudioSource>[] _players;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            AudioSource[] sources = GetComponents<AudioSource>();
            _players = new (bool, AudioSource)[sources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                _players[i] = (false, sources[i]);
            }
            //默认播放背景声音
            PlayAudio(EventAudio.Background);

            StartCoroutine(CheckIsPlaying());
        }


        public void SetVolume(EventAudio audioType, float voluem)
        {
            GameAudio gameAudio = _audios.Find(a => a.audioType == audioType);
            gameAudio.volume = voluem;
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].Item2.clip == gameAudio.clip)
                {
                    _players[i].Item2.volume = voluem;
                    return;
                }
            }
        }


        public void PlayAudio(EventAudio audioType)
        {
            GameAudio gameAudio = _audios.Find(a => a.audioType == audioType);
            for (int i = 0; i < _players.Length; i++)
            {
                if (!_players[i].Item1)
                {
                    _players[i].Item1 = true;
                    _players[i].Item2.clip = gameAudio.clip;
                    _players[i].Item2.volume = gameAudio.volume;
                    _players[i].Item2.loop = !gameAudio.justOne;
                    _players[i].Item2.Play();
                    return;
                }
            }
        }

        public void StopAudio(EventAudio audioType)
        {
            GameAudio gameAudio = _audios.Find(a => a.audioType == audioType);
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].Item2.clip == gameAudio.clip)
                {
                    _players[i].Item2.Stop();
                    _players[i].Item1 = false;
                    return;
                }
            }
        }

        private IEnumerator CheckIsPlaying()
        {
            WaitForSeconds waitFor = new WaitForSeconds(0.05f);
            while (true)
            {
                for (int i = 0; i < _players.Length; i++)
                {
                    if (!_players[i].Item2.isPlaying)
                    {
                        _players[i].Item1 = false;
                    }
                }
                yield return waitFor;
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }

    public enum EventAudio
    {
        /// <summary>
        /// 背景声音
        /// </summary>
        Background,

        /// <summary>
        /// 脚步声音
        /// </summary>
        Walk,

        /// <summary>
        /// 切菜的声音
        /// </summary>
        Chop,

        /// <summary>
        /// 警告声音
        /// </summary>
        Warn,

        /// <summary>
        /// 放下物品的声音
        /// </summary>
        Drop,

        /// <summary>
        /// 将物品丢弃到垃圾桶的声音
        /// </summary>
        Trash,

        /// <summary>
        /// 煎肉的声音
        /// </summary>
        Sizzle,

        /// <summary>
        /// 拿起物品的声音
        /// </summary>
        Pickup,

        /// <summary>
        /// 上菜成功的声音
        /// </summary>
        Success,

        /// <summary>
        /// 上菜失败的声音
        /// </summary>
        Fail,
    }
}

