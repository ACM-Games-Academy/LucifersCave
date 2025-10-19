using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float attackRange;
    public float attackDamage;
    public bool playerInAttack;
    public float attackDelay;
    bool isAttacking;

    [Header("Animation")]
    public Animator animator;

    [Header("References")]
    private EnemyHealth enemyHealth;
    public PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();


        if (enemyHealth != null )
        {
            enemyHealth.OnDeathEvent += HandleDeath;
        }
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.isDead) return; 
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
            StartCoroutine(PlayerDamage());
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

    public IEnumerator PlayerDamage()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackDelay);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        isAttacking = false;
    }

    public void Initialize(Transform player)
    {
        this.player = player;
        playerHealth = player.GetComponent<PlayerHealth>();
    }
}

