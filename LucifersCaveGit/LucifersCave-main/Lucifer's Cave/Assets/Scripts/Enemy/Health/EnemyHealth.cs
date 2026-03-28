using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, HealthBase, IDamageable
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

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    [Header("References")]
    private Collider col;
    private Rigidbody rb;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        isHurt = true;

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

    public void Death()
    {
        if (isDead) return;
        isDead = true;

        RandomiseAnimation();
        animator.SetInteger("DeathInt", randomDeathIndex);
        animator.SetTrigger("isDead");

        if (agent != null)
        {
            if (agent.isOnNavMesh)
                agent.isStopped = true;
                agent.ResetPath();
        }

        if (rb != null) rb.isKinematic = true;

        if (col != null) col.enabled = false;

        OnDeathEvent?.Invoke();
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        OnDeathEvent = null;
    }
}