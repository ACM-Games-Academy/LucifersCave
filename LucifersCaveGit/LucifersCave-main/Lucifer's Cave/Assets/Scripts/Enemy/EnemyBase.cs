using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform player;
    public float spottingDistance;
    public EnemyHealth health;

    [Header("Animations")]
    Animator animator;
    [SerializeField] private float speedDamp = 0.15f;
    private int randomWalkIndex;
    private bool hasStartedWalking = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        agent.isStopped = true;
        RandomiseAnimation();

        EnsureOnNavMesh(agent);

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    public void RandomiseAnimation()
    {
        randomWalkIndex = Random.Range(0, 3);
    }
    
    void Update()
    {
        if (health != null && health.isDead) return; 
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer < spottingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            EnemyMovement();
        }
        else
        {
            hasStartedWalking = false;
            agent.isStopped = true;
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
        }
    }

    private void EnemyMovement()
    {
        if (hasStartedWalking) return;

        animator.SetInteger("WalkInt", randomWalkIndex);
        hasStartedWalking = true;
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

