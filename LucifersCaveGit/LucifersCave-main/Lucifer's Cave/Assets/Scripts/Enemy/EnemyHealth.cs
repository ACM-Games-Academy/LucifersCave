using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [HideInInspector] public int currentHealth;
    public int maxHealth;
    public int knifePoints;

    [Header("Animator")]
    Animator animator;
    NavMeshAgent agent;
    private int randomDeathIndex;

    public bool isDead {  get; private set; }
    public event Action OnDeathEvent;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //RandomiseAnimation();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    /*
    public void RandomiseAnimation()
    {
        randomDeathIndex = UnityEngine.Random.Range(0, 2);
    }
    */

    public void Death()
    {
        if (isDead) return;
        isDead = true;

        //animator.SetInteger("DeathInt", randomDeathIndex);
        animator.SetTrigger("isDead");

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        OnDeathEvent?.Invoke();
        Destroy(transform.parent.gameObject, 5f);
    }
}