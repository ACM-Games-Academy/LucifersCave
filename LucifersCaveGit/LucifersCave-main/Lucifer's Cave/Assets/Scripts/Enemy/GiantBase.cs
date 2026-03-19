using System.Collections;
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
    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;

    [Header("Patroling")]
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    private float walkPointTimer;

    [Header("Attacking")]
    public float damage;
    public bool alreadyAttacked;
    public float attackDelay;

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
        WalkPointTimer();
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, 
            transform.position.y, 
            transform.position.z + randomZ);

        Vector3 rayStart = walkPoint + Vector3.up * 5;

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit floorHit, 10, whatIsGround))
        {
            walkPoint = floorHit.point;

            Vector3 direction = (walkPoint - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, walkPoint);

            if (!Physics.Raycast(transform.position, direction, distance, whatIsObstacle))
            {
                walkPointSet = true;
                WaitAtWayPoint();
            }
        }
    }

    private void WalkPointTimer()
    {
        if (walkPointSet)
        {
            walkPointTimer += Time.deltaTime;
        }
        if (walkPointTimer > 5f)
        {
            walkPointSet = false;
            resetTimer();
            SearchWalkPoint();
        }
    }

    public void resetTimer()
    {
        walkPointTimer = 0;
    }

    public abstract void ChasePlayer();

    public abstract void AttackPlayer();

    public void EnemyMovement()
    {
        if (hasStartedWalking) return;

        animator.SetBool("isRunning", true);
        hasStartedWalking = true;

        if (agent.GetComponent<Rigidbody>().linearVelocity.magnitude < 0.1f)
        {
            animator.SetBool("isRunning", false);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    IEnumerator WaitAtWayPoint()
    {
        agent.isStopped = true;
        animator.SetBool("isRunning", false);
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
        SearchWalkPoint();
        animator.SetBool("isRunning", true);
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