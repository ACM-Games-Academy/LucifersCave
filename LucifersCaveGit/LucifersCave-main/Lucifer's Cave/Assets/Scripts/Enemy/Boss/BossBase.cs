using UnityEngine;
using UnityEngine.AI;

public abstract class BossBase : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform player;
    public EnemyHealth health;

    [Header("Animations")]
    Animator animator;
    [SerializeField] private float speedDamp = 0.15f;

    public enum BossState
    {
        Idle,
        Chasing,
        Stomping,
        Jumping,
        MagicAttacking,
        Reinforcements
    }

    public BossState state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        EnsureOnNavMesh(agent);

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    void Update()
    {
        if (health != null && health.isDead) return;
        if (player == null || agent == null || !agent.isOnNavMesh) return;
        if (PlayerHealth.isDead) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        agent.SetDestination(player.position);

        bool isMoving = distanceToPlayer > agent.stoppingDistance;

        animator.SetBool("isMoving", isMoving);
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

public interface IMagicAttack
{
    void CastMagic();
}

public interface IStompAttack
{
    void Stomp();
}

public interface IJumpAttack
{
    void JumpAttack();
}

public interface ICallReinforcements
{
    void CallReinforcements();
}