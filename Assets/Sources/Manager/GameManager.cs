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
        private bool _isReady = false;
        public bool isReady { get { return _isReady; } }

        public Dictionary<uint, VegetableConfig>    vegetableIdToConfig = null;
        public Dictionary<uint, IncidentConfig>     incidentIdToConfig  = null;
        public Dictionary<uint, ResponseConfig>     responseIdToConfig  = null;


        public void SetReady()
        {
            _isReady = true;

            Initialized?.Invoke(this, null);
        }
    }
}