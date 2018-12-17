using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public Camera mainCam = null;

    private NavMeshAgent _navMeshAgent = null;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit camToLevelHit))
        {
            _navMeshAgent.SetDestination(camToLevelHit.point);
        }
    }
}
