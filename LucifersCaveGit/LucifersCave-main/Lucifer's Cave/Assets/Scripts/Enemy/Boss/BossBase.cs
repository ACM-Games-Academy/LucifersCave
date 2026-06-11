using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BossBase : MonoBehaviour
{
    private static readonly int IsIdleHash = Animator.StringToHash("isIdle");
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;
    public GiantHealthBoss health;

    [Header("Animations")]
    [HideInInspector] public Animator animator;

    public enum BossState
    {
        Roaring,
        Idle,
        Chasing,
        Stomping,
        Jumping,
        MagicAttacking,
        Reinforcements,
        Dead
    }

    public BossState state;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        EnsureOnNavMesh(agent);

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    protected virtual void Update()
    {
        if (health != null && health.isDead) return;
        if (player == null || agent == null || !agent.isOnNavMesh) return;
        if (PlayerHealth.isDead) return;

        if (state == BossState.Chasing)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("isMoving",
            state == BossState.Chasing &&
            !agent.isStopped &&
            agent.velocity.sqrMagnitude > 0.01f);
        }

        if (state == BossState.Idle)
        {
            agent.isStopped = true;
            animator.SetBool(IsIdleHash, state == BossState.Idle);
        }
    }

    public void Initialize(Transform player)
    {
        this.player = player;
    }

    private void HandleDeath()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            state = BossState.Dead;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnDeathEvent -= HandleDeath;
        }
    }

    public void EnsureOnNavMesh(NavMeshAgent agent, float distance = 2f)
    {
        if (agent == null || agent.isOnNavMesh)
        {
            return;
        }

        if (NavMesh.SamplePosition(agent.transform.position, out var hit, distance, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }
}

public interface IMagicAttack
{
    IEnumerator MagicAttack();
}

public interface IStompAttack
{
    IEnumerator StompAttack();
}

public interface IJumpAttack
{
    IEnumerator JumpAttack();
}

public interface ICallReinforcements
{
    IEnumerator CallReinforcements();
}