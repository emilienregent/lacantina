using La.Cantina.Data;
using La.Cantina.Enums;
using La.Cantina.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class StartController : MonoBehaviour
    {
        private const float TIME_TO_FADE_OUT = 2;
        private const int TIME_BEFORE_START = 3;
        private float _elapsedTime = 0;
        private int _timerCountDown = TIME_BEFORE_START;
        private int _playersCount = 0;
        private string _defaultTimerText = "Press A to join";
        private string _startTimerText = "Let's go !";

        [HideInInspector]
        public int _playersReady = 0;

        public GameObject m_HowToPlayRoot;
        public GameObject m_PlayersReadyRoot;

        public Text m_TimerText;
        public List<StartPlayerController> m_Players;
        public SettingsScriptableObject settings;

        private AudioSource _audioSource;

        private void Start()
        {
            settings.numRounds = GameManager.FIRST_ROUND;

            HideHowToPlayScreen();
            _audioSource = GetComponent<AudioSource>();

            // Detect players plugged. Then increment _playersCount accordingly
            for (int i = 0; i < Input.GetJoystickNames().Length; ++i)
            {
                if (Input.GetJoystickNames()[0] != string.Empty)
                {
                    UnityEngine.Debug.Log("Assign Joystick '" + Input.GetJoystickNames()[0] + "' to player " + (i + 1));

                    StartPlayerController player = m_Players[_playersCount];
                    player.InitPlayer(i, this);

                    _playersCount++;
                }
                else
                {
                    UnityEngine.Debug.Log("Joystick '" + i + "' is invalid and can't be use for players");
                }

                if (_playersCount >= m_Players.Count)
                {
                    break;
                }
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
                        SceneManager.LoadScene((int)SceneEnum.ROUND);
                    }
                }

                if (_timerCountDown < TIME_BEFORE_START - 1)
                {
                    FadeOutMusic();
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
                ShowHowToPlayScreen();
            }
        }

        public void CallBackPlayerUnready(int playerID)
        {
            _playersReady--;
            m_TimerText.text = _defaultTimerText;
        }

        public void HideHowToPlayScreen()
        {
            m_HowToPlayRoot.gameObject.SetActive(false);
        }

        public void ShowHowToPlayScreen()
        {
            m_PlayersReadyRoot.gameObject.SetActive(false);
            m_HowToPlayRoot.gameObject.SetActive(true);
        }

        public void FadeOutMusic()
        {
            if(_audioSource.volume > 0)
            {
                _audioSource.volume = _audioSource.volume - (Time.deltaTime / TIME_TO_FADE_OUT);
            }
        }
    }
}