using UnityEngine;

public class Environment : MonoBehaviour
{
    public Seating seating = null;
    public Child[] children = new Child[0];

    public void Start()
    {
        foreach (Child kid in children)
            kid.GoSit(seating);
    }
}
