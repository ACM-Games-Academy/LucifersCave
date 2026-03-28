using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Abomination : GiantBase
{
    public float timeBetweenAttacks;
    private Coroutine attackCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<EnemyHealth>();
        EnsureOnNavMesh(agent);

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    void Update()
    {
        if (PauseMenu.isPaused) return;

        if (health != null && health.isDead)
        {
            agent.isStopped = true;
            return;
        }

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

        if (!alreadyAttacked && attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackFlow());
            attackCoroutine = null;
        }

        agent.isStopped = false;
    }

    IEnumerator AttackFlow()
    { 
        yield return new WaitForSeconds(attackDelay);

        if (health.isDead) yield break;
        if (player == null) yield break;
        if (Vector3.Distance(transform.position, player.position) > attackRange) yield break;

        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(timeBetweenAttacks);

        alreadyAttacked = false;
    }

    public override void ChasePlayer()
    {
        walkPointSet = false;
        CancelAttack();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    public override void Patroling()
    {
        CancelAttack();
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
            StartCoroutine(WaitAtWayPoint());
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeathEvent -= HandleDeath;
        }
    }

    void CancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        alreadyAttacked = false;
    }
}