using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool isClaimed = false;
    public Transform LookAtWhenSitting = null;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, .3f);
    }
}
