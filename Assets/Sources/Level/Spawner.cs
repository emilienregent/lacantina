using UnityEngine;

namespace La.Cantina.Level
{
    public class Spawner : MonoBehaviour
    {
        public Child[] children = new Child[0];
        private bool _childrenLeaving = false;

        public Child SpawnChild()
        {
            for (int i = 0; i < children.Length; ++i)
            {
                if (!children[i].gameObject.activeInHierarchy)
                {
                    children[i].transform.position = transform.position;
                    children[i].gameObject.SetActive(true);

                    return children[i];
                }
            }

            return null;
        }

        public void ReturnAllToStart()
        {
            if (_childrenLeaving)
                return;

            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i].gameObject.activeInHierarchy)
                    children[i].CancelAndLeave(transform.position);
            }

            _childrenLeaving = true;
        }
    }
}