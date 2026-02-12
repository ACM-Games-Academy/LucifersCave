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
    public float attackRange = 2f;
    public float attackDamage = 25f;
    public float attackDelay;

    private Coroutine attackCoroutine;

    [Header("Animation")]
    private bool onCooldown;
    public bool isAttacking;
    public Animator animator;

    private EnemyHealth enemyHealth;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null )
        {
            enemyHealth.OnDeathEvent += HandleDeath;
        }

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        onCooldown = false;
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeathEvent -= HandleDeath;
        }
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.isDead) return; 
        if (player == null) return;

        bool playerInAttack = Vector3.Distance(transform.position, player.position) <= attackRange;

        if (playerInAttack)
            TryAttackPlayer();
        else ResumeMovement();
    }

    private void TryAttackPlayer()
    {
        StopMovementAndFacePlayer();

        if (onCooldown) return;

        attackCoroutine = StartCoroutine(AttackFlow());
        isAttacking = true;
    }

    private IEnumerator AttackFlow()
    {
        onCooldown = true;

        if (animator != null)
            animator.SetTrigger("AttackTrigg");

        yield return new WaitForSeconds(attackDelay);

        if (enemyHealth != null && enemyHealth.isDead) yield break;
        if (player == null) yield break;
        if (playerHealth == null) 
            GameObject.FindWithTag("Player")?.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            playerHealth = player.GetComponent<PlayerHealth>();

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(timeBetweenAttacks);

        onCooldown = false;
        attackCoroutine = null;
        isAttacking = false;
    }

    private void ResumeMovement()
    {
        if (agent != null && agent.isStopped)
        {
            agent.isStopped = false;
        }
    }

    private void HandleDeath()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        onCooldown = true;
    }

    private void StopMovementAndFacePlayer()
    {
        if (agent != null && !agent.isStopped)
            agent.isStopped = true;

        Vector3 direction = (player.position - transform.position);
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion target = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 8f);
        }
    }

    public void Initialize(Transform player)
    {
        this.player = player;
        playerHealth = player.GetComponent<PlayerHealth>();
    }
}

