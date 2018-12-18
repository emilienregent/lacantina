using UnityEngine;
using UnityEngine.AI;

public class Child : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    private Seat currentSeat = null;

    public void GoSit(Seating seats)
    {
        currentSeat = seats.GetEmptySeat();

        if (currentSeat != null)
        {
            currentSeat.isClaimed = true;
            _navMeshAgent.SetDestination(currentSeat.seatTransform.position);
        }
    }
}
