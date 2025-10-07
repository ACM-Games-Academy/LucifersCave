using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image playerHealthBar;

    void Update()
    {
        if (playerHealth != null && playerHealthBar != null)
        {
            float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;
        }
    }
}
