using La.Cantina.Level;
using System;
using UnityEngine;

namespace La.Cantina.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public int playerNumber = 1;
        private Spawner _spawner = null;
        private bool _fullSpawned = false;

        [SerializeField]
        private int _fullSpawnDelay = 15;

        private void Awake()
        {
            if (GameManager.instance.isReady == false)
                GameManager.instance.Initialized += OnGameInitialized;
            else
                OnGameInitialized(GameManager.instance, null);

            GameManager.instance.TimerUpdated += OnTimerUpdated;
        }

        private void OnGameInitialized(object sender, EventArgs eventArgs)
        {


            GameObject[] spawners = GameObject.FindGameObjectsWithTag(string.Format("SpawnerPlayer{0}", playerNumber));
            GameObject[] seatings = GameObject.FindGameObjectsWithTag(string.Format("SeatingPlayer{0}", playerNumber));

            if (seatings.Length == 0)
                Debug.LogError(string.Format("No seating found for player {0}", playerNumber));

            if (spawners.Length == 0)
                Debug.LogError(string.Format("No spawner found for player {0}", playerNumber));

            _spawner = spawners[0].GetComponent<Spawner>();
            Seating seating = seatings[0].GetComponent<Seating>();

            for (int i = 0; i < _spawner.children.Length; ++i)
            {
                _spawner.children[i].allowedSeating = seating;
                _spawner.children[i].InitForPlayer(playerNumber);
            }

            AddChild();
        }

        private void OnTimerUpdated(object sender, int elapsedtime)
        {
            if (!_fullSpawned && elapsedtime > _fullSpawnDelay)
            {
                AddChild(GameManager.instance.currentLevelConfig.children_per_player - 1);
                _fullSpawned = true;
            }
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
    }


}