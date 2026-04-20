using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MannequinBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform player;
    private IsEnemyInView isEnemyInViewScript;
    public float spottingDistance;
    public EnemyHealth health;
    float checkTimer;

    [Header("Animations")]
    Animator animator;
    private bool hasStartedWalking = false;
    private bool hasGottenUp = false;
    public bool isInView;
    bool isActive = false;

    Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        isEnemyInViewScript = player.GetComponent<IsEnemyInView>();
        agent.isStopped = true;
        agent.updateRotation = false;

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
        if (!isActive) return;

        if (!hasGottenUp && Vector3.Distance(transform.position, player.position) < spottingDistance)
        {
            StartCoroutine(GetUp());
        }

        checkTimer += Time.deltaTime;

        if (checkTimer > 0.25f)
        {
            checkTimer = 0;
            float viewDistance;
            isInView = isEnemyInViewScript.isEnemyInView(this, out viewDistance);
        }

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer > 30f)
        {
            agent.isStopped = true;
            return;
        }

        if (distanceToPlayer < spottingDistance && !isInView)
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

        if (!isInView && distanceToPlayer > 25f) 
        { 
            foreach (Renderer r in renderers)
            {
                r.enabled = false;
            }
        }
        else
        {
            foreach (Renderer r in renderers)
            {
                r.enabled = true;
            }
        }

        if (isInView)
        {
            transform.rotation = Quaternion.LookRotation(player.position - transform.position);
        }

        bool shouldMove = !agent.isStopped &&
            !agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance > agent.stoppingDistance + 0.05f &&
            !isInView;

        animator.SetBool("isMoving", shouldMove);
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

    public IEnumerator GetUp()
    {
        agent.isStopped = true;
        animator.SetTrigger("hasGotUp");

        yield return new WaitForSeconds(1.17f);

        hasGottenUp = true;
        isActive = true;
        agent.isStopped = false;
    }
}
