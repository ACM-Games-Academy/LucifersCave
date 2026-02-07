using System;
using UnityEngine;
using UnityEngine.AI;

public class GiantHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    [Header("Animator")]
    Animator animator;

    public bool isDead { get; private set; }
    public event Action OnGiantDeathEvent;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }


    public void Death()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("isDead");

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        OnGiantDeathEvent?.Invoke();
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        OnGiantDeathEvent = null;
    }
}