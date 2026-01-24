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
    private int randomWalkIndex;
    private bool hasStartedWalking = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.isStopped = true;
        RandomiseAnimation();

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    public void RandomiseAnimation()
    {
        randomWalkIndex = Random.Range(0, 2);
    }
    
    void Update()
    {
        if (health != null && health.isDead) return; 
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer < spottingDistance)
        {
            EnemyMovement();
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            animator.SetBool("isMoving", false);
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
        animator.SetBool("isMoving", true);
        hasStartedWalking = true;
        agent.isStopped = false;
    }
}

