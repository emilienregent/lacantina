using La.Cantina.Data;
using La.Cantina.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class StartController : MonoBehaviour
    {
        private const int TIME_BEFORE_START = 3;
        private float _elapsedTime = 0;
        private int _timerCountDown = TIME_BEFORE_START;
        private int _playersCount = 0;
        private string _defaultTimerText = "Press A to join";
        private string _startTimerText = "Let's go !";

        [HideInInspector]
        public int _playersReady = 0;

        public Text m_TimerText;
        public List<StartPlayerController> m_Players;
        public SettingsScriptableObject settings;

        private void Start()
        {
            // Detect players plugged. Then increment _playersCount accordingly
            _playersCount = Input.GetJoystickNames().Length;

            // 4 controllers max
            int numberControllers = Mathf.Max(Input.GetJoystickNames().Length, m_Players.Count);
            for(int i = 0; i < numberControllers; i++)
            {
                StartPlayerController player = m_Players[i];
                player.InitPlayer(i, this);
            }
        }

        private void Update()
        {
            if (_playersReady == _playersCount)
            {
                // Each second
                _elapsedTime += Time.deltaTime;

                if (_elapsedTime >= 1f)
                {

                    _elapsedTime = _elapsedTime % 1f;
                    _timerCountDown--;

                    if (_timerCountDown > 0)
                    {
                        m_TimerText.text = _timerCountDown.ToString();
                    } else if(_timerCountDown == 0)
                    {
                        m_TimerText.text = _startTimerText;
                    } else
                    {
                        settings.numPlayers = _playersReady;
                        SceneManager.LoadScene((int)SceneEnum.GAME);
                    }
                }
            } else
            {
                _elapsedTime = 0;
                _timerCountDown = TIME_BEFORE_START;
                m_TimerText.text = _defaultTimerText;
            }
        }

        public void CallBackPlayerReady(int playerID)
        {
            _playersReady++;
            if(_playersReady == _playersCount)
            {
                m_TimerText.text = _timerCountDown.ToString();
            }
        }

        public void CallBackPlayerUnready(int playerID)
        {
            _playersReady--;
            m_TimerText.text = _defaultTimerText;
        }
    }
}