using UnityEngine;

public class Seat : MonoBehaviour
{
    public Transform seatTransform = null;
    public bool isClaimed = false;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(seatTransform.position, .3f);
    }
}
