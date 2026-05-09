using UnityEngine;

public class AlienAI : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2f;
    public float runDistance = 4f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 targetPos = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.LookAt(targetPos);

        if (distance > runDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * 2 * Time.deltaTime
            );

            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }
    }
}