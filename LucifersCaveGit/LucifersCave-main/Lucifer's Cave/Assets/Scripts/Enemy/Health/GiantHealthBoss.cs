using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(GiantHealthBar))]
public class GiantHealthBoss : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    [Header("Animator")]
    Animator animator;
    NavMeshAgent agent;

    public bool isDead { get; private set; }
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

        if (GetComponent<GiantHealthBar>() != null)
        {
            GetComponent<GiantHealthBar>().UpdateBar(currentHealth, maxHealth);
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
        isHurt = false;
    }

    public void Death()
    {
        if (isDead) return;
        isDead = true;

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
        Destroy(gameObject, 8f);
    }

    private void OnDestroy()
    {
        OnDeathEvent = null;
    }
}