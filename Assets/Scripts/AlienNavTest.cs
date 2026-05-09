using UnityEngine;
using UnityEngine.AI;

public class AlienNavTest : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        Debug.Log("Alien on NavMesh: " + agent.isOnNavMesh);
    }

    void Update()
    {
        if (target == null)
        {
            Debug.LogError("Target is missing!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Alien is not on NavMesh!");
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(target.position);

        Debug.Log("Distance to player: " + agent.remainingDistance);
    }
}