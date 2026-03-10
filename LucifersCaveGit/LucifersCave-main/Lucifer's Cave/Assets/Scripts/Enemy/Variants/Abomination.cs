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
        agent.isStopped = true;
        RandomiseAnimation();
        EnemyMovement();

        EnsureOnNavMesh(agent);

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
        if (playerInSight && !playerInAttack)
        {
            ChasePlayer();
        }
        if (playerInSight && playerInAttack)
        {
            AttackPlayer();
        }
    }

    public override void RandomiseAnimation()
    {
        randomWalkIndex = Random.Range(0, 3);
    }

    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
