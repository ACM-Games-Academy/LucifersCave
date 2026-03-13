using UnityEngine;
using UnityEngine.AI;

public class Abomination : GiantBase
{
    public float timeBetweenAttacks;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<EnemyHealth>();
        EnsureOnNavMesh(agent);
        EnemyMovement();
        Patroling();


        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSight && !playerInAttack)
        {
            Patroling();
        }
        else if (playerInSight && !playerInAttack)
        {
            ChasePlayer();
        }
        else if (playerInSight && playerInAttack)
        {
            AttackPlayer();
        } 
        else
        {
            return;
        }
    }

    public override void AttackPlayer()
    {
        agent.isStopped = true;

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Patroling()
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

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeathEvent -= HandleDeath;
        }
    }
}