using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public Image playerHealthBar;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.OnHealthChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateBar;
    }

    public void UpdateBar(float current, float max)
    {
        playerHealthBar.fillAmount = current / max;
    }
}
