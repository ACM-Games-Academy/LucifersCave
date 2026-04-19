using UnityEngine;
using UnityEngine.UI;

public class GiantHealthBar : MonoBehaviour
{
    private EnemyHealth GiantHealth;
    private Image giantHealthBar;

    [Header("Distance checks")]
    private Transform player;
    private GameObject healthBarCanvas;
    public float displayDistance = 20f;

    private void Start()
    {
        GiantHealth = GetComponent<EnemyHealth>();

        if (GiantHealth != null)
            UpdateBar(GiantHealth.CurrentHealth, GiantHealth.MaxHealth);
    }

    private void Update()
    {
        if (player == null || giantHealthBar == null || PlayerHealth.isDead)
            return;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= displayDistance)
            EnableHealthBar();
        else
            DisableHealthBar();
    }

    public void UpdateBar(float current, float max)
    {
        giantHealthBar.fillAmount = current / max;
    }

    public void EnableHealthBar()
    {
        healthBarCanvas.SetActive(true);
    }

    public void DisableHealthBar()
    {
        healthBarCanvas.SetActive(false);
    }

    public void Initialize(Image healthBar, Transform player, GameObject healthBarCanvas)
    {
        giantHealthBar = healthBar;
        this.player = player;
        this.healthBarCanvas = healthBarCanvas;
    }
}
