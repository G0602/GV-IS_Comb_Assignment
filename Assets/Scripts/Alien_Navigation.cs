using UnityEngine;
using UnityEngine.AI;

public class AlienNavTest : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;
    private Animator animator;

    private int speedHash;
    private int motionSpeedHash;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        speedHash = Animator.StringToHash("Speed");
        motionSpeedHash = Animator.StringToHash("MotionSpeed");

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update()
    {
        if (target == null || !agent.isOnNavMesh)
            return;

        agent.SetDestination(target.position);

        float speed = agent.velocity.magnitude;

        if (animator != null)
        {
            animator.SetFloat(speedHash, speed);
            animator.SetFloat(motionSpeedHash, speed > 0.1f ? 1f : 0f);
        }
    }
}