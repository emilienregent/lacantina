using La.Cantina.Level;
using UnityEngine;

namespace La.Cantina.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public int playerNumber = 1;
        public Seating seating = null;
        public Child[] children = new Child[0];
        public Spawner spawner = null;

        [SerializeField]
        private float _fullSpawnDelay = 15f;

        public void Start()
        {
            spawner.children = children;

            for (int i = 0; i < children.Length; ++i)
            {
                children[i].allowedSeating = seating;
                children[i].InitForPlayer(playerNumber);
            }

            AddChild();
        }

        public void Update()
        {
            if (_fullSpawnDelay > 0f)
            {
                _fullSpawnDelay -= Time.deltaTime;

                if (_fullSpawnDelay < 0f)
                    AddChild(5);
            }
        }

        public void AddChild(int number = 1)
        {
            for (int i = 0; i < number; ++i)
            {
                Child child = spawner.SpawnChild();
                child.GoSit();
            }
        }
    }


}