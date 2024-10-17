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
            //Ĭ�ϲ��ű�������
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
        /// ��������
        /// </summary>
        Background,

        /// <summary>
        /// �Ų�����
        /// </summary>
        Walk,

        /// <summary>
        /// �в˵�����
        /// </summary>
        Chop,

        /// <summary>
        /// ��������
        /// </summary>
        Warn,

        /// <summary>
        /// ������Ʒ������
        /// </summary>
        Drop,

        /// <summary>
        /// ����Ʒ����������Ͱ������
        /// </summary>
        Trash,

        /// <summary>
        /// ���������
        /// </summary>
        Sizzle,

        /// <summary>
        /// ������Ʒ������
        /// </summary>
        Pickup,

        /// <summary>
        /// �ϲ˳ɹ�������
        /// </summary>
        Success,

        /// <summary>
        /// �ϲ�ʧ�ܵ�����
        /// </summary>
        Fail,
    }
}

