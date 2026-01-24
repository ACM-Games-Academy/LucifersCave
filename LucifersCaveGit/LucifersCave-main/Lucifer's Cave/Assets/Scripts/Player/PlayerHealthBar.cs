using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image playerHealthBar;

    private void Start()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float current, float max)
    {
        playerHealthBar.fillAmount = current / max;
    }
}
