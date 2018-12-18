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

        public void AddChild()
        {
            Child child = spawner.SpawnChild();
            child.GoSit();
        }
    }


}