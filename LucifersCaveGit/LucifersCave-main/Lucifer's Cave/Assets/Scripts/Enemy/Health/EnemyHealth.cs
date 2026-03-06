using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : HealthBase, IDamageable
{
    public int currentHealth;
    public int maxHealth;
    public int knifePoints;

    [Header("Animator")]
    Animator animator;
    NavMeshAgent agent;
    private int randomDeathIndex;

    public bool isDead {  get; private set; }
    public bool isHurt = false;
    public event Action OnDeathEvent;

    public override int CurrentHealth => currentHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        RandomiseAnimation();
    }

    public override void TakeDamage(int amount)
    {
        isHurt = true;
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
        isHurt = false;
    }

    
    public void RandomiseAnimation()
    {
        randomDeathIndex = UnityEngine.Random.Range(0, 2);
    }

    public override void Death()
    {
        if (isDead) return;
        isDead = true;

        animator.SetInteger("DeathInt", randomDeathIndex);
        animator.SetTrigger("isDead");

        if (agent != null)
        {
            if (agent.isOnNavMesh)
                agent.isStopped = true;

            agent.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        OnDeathEvent?.Invoke();
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        OnDeathEvent = null;
    }
}