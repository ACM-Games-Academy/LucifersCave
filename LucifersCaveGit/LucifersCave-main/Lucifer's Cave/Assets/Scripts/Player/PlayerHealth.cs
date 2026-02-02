using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<float, float> OnHealthChanged;

    [Header("Health")]
    private float currentHealth;
    [SerializeField] private float maxHealth;

    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Death()
    {
        isDead = true;
        Destroy(gameObject);
    }
}