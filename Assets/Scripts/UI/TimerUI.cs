using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class TimerUI : MonoBehaviour
    {
        [SerializeField] private Image _timerImg;
        [SerializeField] private TMP_Text _timerTxt;
        [Range(60,300)]
        [SerializeField] private int TIMER = 60;

        private bool _isStartGame;
        private int _timer;

        public int Time { set => TIMER = value; } 

        private void Awake()
        {
            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame, false);
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
            _timer = TIMER;
            _isStartGame = true;
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while(_timer > 0)
            {
                while (!_isStartGame) { yield return null; }

                yield return new WaitForSeconds(1f);

                _timer--;
                _timerTxt.text = _timer.ToString();
                _timerImg.fillAmount = ((float)(TIMER - _timer)) / TIMER;
            }

            //重置
            _timer = TIMER;
            _timerImg.fillAmount = 0;
            _timerTxt.text = TIMER.ToString();
            _isStartGame = false;

            EventManager.Instance.TriggerEvent(GameEvent.GameOver);
        }

    }
}
