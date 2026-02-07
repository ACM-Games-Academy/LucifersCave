using UnityEngine;
using UnityEngine.UI;

public class GiantHealthBar : MonoBehaviour
{
    private GiantHealth GiantHealth;
    private Image giantHealthBar;

    private void Start()
    {
        GiantHealth = GetComponent<GiantHealth>();

        if (GiantHealth != null)
            GiantHealth.OnHealthChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        if (GiantHealth != null)
            GiantHealth.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float current, float max)
    {
        giantHealthBar.fillAmount = current / max;
    }

    public void Initialize(Image healthBar)
    {
        giantHealthBar = healthBar;
    }
}
