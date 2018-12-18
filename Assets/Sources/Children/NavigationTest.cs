using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public Camera mainCam = null;

    [SerializeField]
    private NavMeshAgent _navMeshAgent = null;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit camToLevelHit))
        {
            _navMeshAgent.SetDestination(camToLevelHit.point);
        }
    }
}
