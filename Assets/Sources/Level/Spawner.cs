using UnityEngine;

namespace La.Cantina.Level
{
    public class Spawner : MonoBehaviour
    {
        public Child[] children = new Child[0];

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
    }
}