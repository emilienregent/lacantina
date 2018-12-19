using La.Cantina.Types;
using System;
using System.Collections.Generic;

namespace La.Cantina.Manager
{
    public class GameManager
    {
        private static GameManager _instance = null;

        public static GameManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        public event EventHandler Initialized;
        public event EventHandler<int> TimerUpdated;
        public event EventHandler<int> TimerEnded;

        private bool _isReady = false;
        public bool isReady { get { return _isReady; } }

        private int _elapsedTime = 0;
        public int elapsedTime { get { return _elapsedTime; } }

        private LevelConfig _currentLevelConfig = null;
        public LevelConfig currentLevelConfig { get { return _currentLevelConfig; } }

        public Dictionary<uint, VegetableConfig>    vegetableIdToConfig = null;
        public Dictionary<uint, IncidentConfig>     incidentIdToConfig  = null;
        public Dictionary<uint, ResponseConfig>     responseIdToConfig  = null;
        public Dictionary<uint, LevelConfig>        levelIdToConfig     = null;
        public List<uint>                           levelIds            = null;

        public void SetReady()
        {
            uint levelIdSelected = levelIds[0];

            _currentLevelConfig = levelIdToConfig[levelIdSelected];

            _isReady = true;

            Initialized?.Invoke(this, null);
        }

        public void UpdateTimer()
        {
            _elapsedTime++;

            if (_elapsedTime <= _currentLevelConfig.time)
            {
                TimerUpdated?.Invoke(this, _elapsedTime);
            }
            else
            {
                TimerEnded?.Invoke(this, _elapsedTime);
            }
        }
    }
}