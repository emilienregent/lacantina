using La.Cantina.Level;
using System;
using UnityEngine;

namespace La.Cantina.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private int            _playerNumber   = 1;
        [SerializeField] private Material       _playerMaterial = null;
        [SerializeField] private GameObject     _playerPrefab   = null;

        private Spawner     _spawner        = null;
        private Seating     _seating        = null;
        private bool        _fullSpawned    = false;
        private GameObject  _currentPlayer  = null;
        public  Vector3     _spawnPosition  = Vector3.zero;

        [SerializeField]
        private int _fullSpawnDelay = 15;

        private int _score = 0;
        public int score { get { return _score; } }

        public int playerNumber { get { return _playerNumber; } }

        private void Awake()
        {
            if (GameManager.instance.isReady == false)
                GameManager.instance.Initialized += OnGameInitialized;
            else
                OnGameInitialized(GameManager.instance, null);

            GameManager.instance.TimerUpdated += OnTimerUpdated;
            GameManager.instance.TimerEnded += OnTimerEnded;
        }

        private void OnGameInitialized(object sender, EventArgs eventArgs)
        {
            // Spawn Player
            if (_playerNumber <= GameManager.instance.playerCount)
            {
                _currentPlayer = GameObject.Instantiate(_playerPrefab, _spawnPosition, Quaternion.identity);
                _currentPlayer.name = "P" + _playerNumber;

                _currentPlayer.GetComponent<PlayerController>().Initialize(_playerMaterial);
            }

            // Spawn Children
            GameObject[] spawners = GameObject.FindGameObjectsWithTag(string.Format("SpawnerPlayer{0}", _playerNumber));
            GameObject[] seatings = GameObject.FindGameObjectsWithTag(string.Format("SeatingPlayer{0}", _playerNumber));

            if (seatings.Length == 0)
                Debug.LogError(string.Format("No seating found for player {0}", _playerNumber));

            if (spawners.Length == 0)
                Debug.LogError(string.Format("No spawner found for player {0}", _playerNumber));

            _spawner = spawners[0].GetComponent<Spawner>();
            _seating = seatings[0].GetComponent<Seating>();

            for (int i = 0; i < _spawner.children.Length; ++i)
            {
                _spawner.children[i].allowedSeating = _seating;
                _spawner.children[i].InitForPlayer(this);
            }

            AddChild();
        }

        private void OnTimerUpdated(object sender, int elapsedTime)
        {
            if (!_fullSpawned && elapsedTime > _fullSpawnDelay)
            {
                AddChild(GameManager.instance.currentLevelConfig.children_per_player - 1);
                _fullSpawned = true;
            }

#if DEBUG_LEAVE
            if (elapsedTime == 20)
                _spawner.ReturnAllToStart();
#endif
        }

        private void OnTimerEnded(object sender, int elapsedTime)
        {
            _spawner.ReturnAllToStart();
        }

        public void AddChild(int number = 1)
        {
            for (int i = 0; i < number; ++i)
            {
                Child child = _spawner.SpawnChild();

                if (child != null)
                    child.GoSit();
            }
        }

        public void UpdateScore(int points, bool positive = true)
        {
            _score += positive ? points : -points;

            _seating.playerTableCanvas.UpdateScore(points, positive);
        }
    }
}