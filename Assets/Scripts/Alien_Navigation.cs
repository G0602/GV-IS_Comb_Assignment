using UnityEngine;
using UnityEngine.AI;

public class AlienNavTest : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;
    private Animator animator;
    private AlienFreezeTest freezeState;

    private int speedHash;
    private int motionSpeedHash;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        freezeState = GetComponent<AlienFreezeTest>();

        if (freezeState == null)
        {
            freezeState = gameObject.AddComponent<AlienFreezeTest>();
        }

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

        if (freezeState != null && freezeState.IsFrozen())
        {
            agent.isStopped = true;
            UpdateAnimator(0f);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(target.position);

        UpdateAnimator(agent.velocity.magnitude);
    }

    private void UpdateAnimator(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat(speedHash, speed);
            animator.SetFloat(motionSpeedHash, speed > 0.1f ? 1f : 0f);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
    }

    private void OnLand(AnimationEvent animationEvent)
    {
    }
}
