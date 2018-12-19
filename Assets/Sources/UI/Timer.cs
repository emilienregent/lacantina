using La.Cantina.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace La.Cantina.UI
{
    public class Timer : MonoBehaviour
    {
        private const string TIMER_FORMAT = "{0}:{1}";

        [SerializeField] private Text _timerText = null;

        private void Start()
        {
            GameManager.instance.TimerUpdated += OnTimerUpdated;

            if (GameManager.instance.isReady == false)
            {
                GameManager.instance.Initialized += OnGameInitialized;
            }
            else
            {
                OnGameInitialized(GameManager.instance, null);
            }
        }

        private void OnGameInitialized(object sender, EventArgs eventArgs)
        {
            OnTimerUpdated(sender, GameManager.instance.elapsedTime);

            _timerText.enabled = true;
        }

        private void OnTimerUpdated(object sender, int elapsedTime)
        {
            int time        = GameManager.instance.currentLevelConfig.time - elapsedTime;
            int minutes     = Mathf.RoundToInt(time / 60);
            int secondes    = time % 60;

            _timerText.text = string.Format(TIMER_FORMAT, minutes.ToString("00"), secondes.ToString("00"));
        }
    }
}
