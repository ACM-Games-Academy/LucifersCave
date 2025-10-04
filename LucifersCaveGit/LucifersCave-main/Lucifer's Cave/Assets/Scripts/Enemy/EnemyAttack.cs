using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;
    private EnemyHealth enemyHealth;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float attackRange;
    public bool playerInAttack;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null )
        {
            enemyHealth.OnDeathEvent += HandleDeath;
        }
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.isDead) return; // don’t attack if dead
        if (player == null) return;

        playerInAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttack)
        {
            AttackPlayer();
        }
        else
        {
            if (agent != null && agent.isStopped)
            {
                agent.isStopped = false;
            }
        }
    }

    private void AttackPlayer()
    {
        if (agent != null && !agent.isStopped)
        {
            agent.isStopped = true;
        }

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.SetTrigger("AttackTrigg");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void HandleDeath()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    public void Initialize(Transform player)
    {
        this.player = player;
    }
}

