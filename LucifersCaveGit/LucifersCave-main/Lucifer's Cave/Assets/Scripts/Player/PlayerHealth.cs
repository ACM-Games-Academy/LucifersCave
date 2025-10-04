using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}

