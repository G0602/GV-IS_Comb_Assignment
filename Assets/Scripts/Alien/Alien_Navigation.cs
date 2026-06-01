using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienNavTest : MonoBehaviour
{
    public Transform target;
    public GraphNode startNode;
    public GraphNode targetNode;
    public float waypointReachedDistance = 0.75f;
    public float repathInterval = 0.5f;
    public float targetMoveRepathDistance = 2f;
    public float deathDistance = 2f;
    public string playerTag = "Player";
    public PathLineRenderer pathLineRenderer;

    private NavMeshAgent agent;
    private Animator animator;
    private AlienFreezeTest freezeState;
    private readonly List<GraphNode> currentPath = new List<GraphNode>();
    private GraphNode currentNode;
    private GraphNode activeTargetNode;
    private Vector3 lastTargetPosition;
    private int waypointIndex;
    private float nextRepathTime;
    private bool headingToPlayer;

    private int speedHash;
    private int motionSpeedHash;

    private void Start()
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

        RecalculatePath();
    }

    private void Update()
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

        if (ShouldRecalculatePath())
        {
            RecalculatePath();
        }

        FollowAStarPathToPlayer();
        CheckPlayerDistance();
        UpdateAnimator(agent.velocity.magnitude);
    }

    public void RecalculatePath()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (target == null || agent == null || !agent.isOnNavMesh)
        {
            return;
        }

        currentNode = ResolveCurrentNode();
        activeTargetNode = ResolveTargetNode();
        lastTargetPosition = target.position;
        nextRepathTime = Time.time + repathInterval;
        currentPath.Clear();
        waypointIndex = 0;
        headingToPlayer = false;

        List<GraphNode> path;

        if (!mainScript.isDebugMode)
        {
            path = A_Star_Pathfinder.FindPath(currentNode, activeTargetNode);
        }
        else
        {
            path = BFSPathfinder.FindPath(currentNode, activeTargetNode);
            if (pathLineRenderer != null)
            {
                pathLineRenderer.DrawPath(path);
            }
        }

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No" + (!mainScript.isDebugMode ? " A*" : " BFS") + " graph path found for alien.");
            agent.SetDestination(target.position);
            headingToPlayer = true;
            return;
        }

        currentPath.AddRange(path);
        waypointIndex = currentPath.Count > 1 ? 1 : 0;
        SetDestinationToCurrentWaypoint();

        Debug.Log((mainScript.isDebugMode ? "A*" : "BFS") + " alien path: " + FormatPath(currentPath));
    }

    private bool ShouldRecalculatePath()
    {
        if (Time.time < nextRepathTime)
        {
            return false;
        }

        nextRepathTime = Time.time + repathInterval;

        GraphNode nearestTargetNode = ResolveTargetNode();
        bool playerChangedGraphNode = nearestTargetNode != activeTargetNode;
        bool playerMovedEnough = Vector3.Distance(lastTargetPosition, target.position) >= targetMoveRepathDistance;

        return currentPath.Count == 0 || playerChangedGraphNode || playerMovedEnough;
    }

    private void FollowAStarPathToPlayer()
    {
        if (currentPath.Count == 0)
        {
            agent.SetDestination(target.position);
            headingToPlayer = true;
            return;
        }

        if (headingToPlayer)
        {
            agent.SetDestination(target.position);
            return;
        }

        if (agent.pathPending || agent.remainingDistance > waypointReachedDistance)
        {
            return;
        }

        currentNode = currentPath[waypointIndex];

        if (waypointIndex >= currentPath.Count - 1)
        {
            agent.SetDestination(target.position);
            headingToPlayer = true;
            return;
        }

        waypointIndex++;
        SetDestinationToCurrentWaypoint();
    }

    private void SetDestinationToCurrentWaypoint()
    {
        if (currentPath.Count == 0 || waypointIndex >= currentPath.Count)
        {
            return;
        }

        agent.SetDestination(currentPath[waypointIndex].Position);
    }

    private GraphNode ResolveCurrentNode()
    {
        if (currentNode != null)
        {
            return currentNode;
        }

        if (startNode != null)
        {
            return startNode;
        }

        return FindClosestNode(transform.position);
    }

    private GraphNode ResolveTargetNode()
    {
        if (targetNode != null)
        {
            return targetNode;
        }

        return FindClosestNode(target.position);
    }

    private GraphNode FindClosestNode(Vector3 position)
    {
        GraphNode[] nodes = FindObjectsByType<GraphNode>();
        GraphNode closest = null;
        float closestDistance = float.PositiveInfinity;

        foreach (GraphNode node in nodes)
        {
            float distance = Vector3.Distance(position, node.Position);

            if (distance < closestDistance)
            {
                closest = node;
                closestDistance = distance;
            }
        }

        return closest;
    }

    private string FormatPath(List<GraphNode> path)
    {
        List<string> names = new List<string>();

        foreach (GraphNode node in path)
        {
            names.Add(node.name);
        }

        return string.Join(" -> ", names);
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

    private void OnTriggerEnter(Collider other)
    {
        ShowLossIfPlayer(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ShowLossIfPlayer(collision.gameObject);
    }

    private void CheckPlayerDistance()
    {
        if (deathDistance <= 0f)
        {
            return;
        }

        if (Vector3.Distance(transform.position, target.position) <= deathDistance)
        {
            ShowLossIfPlayer(target.gameObject);
        }
    }

    private void ShowLossIfPlayer(GameObject other)
    {
        if (!GameOverMenu.IsGameOver && other.CompareTag(playerTag))
        {
            GameOverMenu.ShowGameOverScreen("You Lost");
        }
    }
}
