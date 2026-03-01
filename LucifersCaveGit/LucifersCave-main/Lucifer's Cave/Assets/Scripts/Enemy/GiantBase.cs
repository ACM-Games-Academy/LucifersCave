using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class GiantBase
{
    [Header("General References")]
    public NavMeshAgent agent;
    private Transform player;
    private GiantHealth health;
    private bool hasStartedWalking = false;

    [Header("Layers")]
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    public float damage;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange;
    public float attackRange;
    public bool playerInSight;
    public bool playerInAttack;

    [Header("Animation")]
    private Animator animator;
    private int randomWalkIndex;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<GiantHealth>();
        agent.isStopped = true;
        RandomiseAnimation();
        EnemyMovement();

        EnsureOnNavMesh(agent);

        if (health != null)
        {
            health.OnGiantDeathEvent += HandleDeath;
        }
    }

    public void RandomiseAnimation()
    {
        randomWalkIndex = Random.Range(0, 3);
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSight && !playerInAttack)
        {
            Patroling();
        }
        if (playerInSight && !playerInAttack)
        {
            ChasePlayer();
        }
        if (playerInSight && playerInAttack)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void EnemyMovement()
    {
        if (hasStartedWalking) return;

        animator.SetInteger("WalkInt", randomWalkIndex);
        hasStartedWalking = true;
    }

    private void ResetAttack()
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

    private void HandleDeath()
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