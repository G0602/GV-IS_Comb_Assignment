using UnityEngine;
using UnityEngine.AI;

public class AlienFreezeTest : MonoBehaviour
{
    private bool isFrozen = false;
    private NavMeshAgent agent;
    private Animator animator;

    private int speedHash;
    private int motionSpeedHash;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedHash = Animator.StringToHash("Speed");
        motionSpeedHash = Animator.StringToHash("MotionSpeed");
    }

    public void SetFrozen(bool frozen)
    {
        if (isFrozen == frozen) return;

        isFrozen = frozen;

        if (isFrozen)
        {
            if (agent != null)
            {
                agent.isStopped = true;
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }

            SetAnimatorSpeed(0f);
            Debug.Log("Alien frozen by flashlight.");
        }
        else
        {
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }

            Debug.Log("Alien unfrozen.");
        }
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }

    private void SetAnimatorSpeed(float speed)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetFloat(speedHash, speed);
        animator.SetFloat(motionSpeedHash, speed > 0.1f ? 1f : 0f);
    }
}
