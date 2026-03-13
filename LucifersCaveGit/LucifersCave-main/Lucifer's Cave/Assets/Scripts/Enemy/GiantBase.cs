using UnityEngine;
using UnityEngine.AI;

public abstract class GiantBase : MonoBehaviour
{
    [Header("General References")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;
    [HideInInspector] public EnemyHealth health;
    private bool hasStartedWalking = false;

    [Header("Layers")]
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float damage;
    public bool alreadyAttacked;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInSight;
    public bool playerInAttack;

    [Header("Animation")]
    [HideInInspector] public Animator animator;
    [HideInInspector] public int randomWalkIndex;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    public abstract void Patroling();

    public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    public abstract void ChasePlayer();

    public abstract void AttackPlayer();

    public void EnemyMovement()
    {
        if (hasStartedWalking) return;

        animator.SetBool("isRunning", true);
        hasStartedWalking = true;
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public void HandleDeath()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    public void EnsureOnNavMesh(NavMeshAgent agent, float distance = 2f)
    {
        if (agent == null || agent.isOnNavMesh)
        {
            return;
        }

        if (NavMesh.SamplePosition(agent.transform.position, out var hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }
}