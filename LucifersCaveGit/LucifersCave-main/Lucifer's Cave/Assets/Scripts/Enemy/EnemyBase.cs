using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    bool isDead;
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
        //RandomiseAnimation();

        if (health != null)
        {
            health.OnDeathEvent += HandleDeath;
        }
    }

    /*public void RandomiseAnimation()
    {
        randomWalkIndex = Random.Range(0, 3);
    }
    */
    void Update()
    {
        if (health != null && health.isDead) return; // don’t move if dead
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (!isDead && distanceToPlayer < spottingDistance)
        {
            if (!hasStartedWalking)
            {
                animator.SetInteger("WalkInt", randomWalkIndex);
                animator.SetBool("isMoving", true);
                hasStartedWalking = true;
            }
            agent.SetDestination(player.position);
        }

        if (Input.GetMouseButton(0))
        {
            health.TakeDamage(200);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        animator.SetTrigger("IsAttacking");
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
}

