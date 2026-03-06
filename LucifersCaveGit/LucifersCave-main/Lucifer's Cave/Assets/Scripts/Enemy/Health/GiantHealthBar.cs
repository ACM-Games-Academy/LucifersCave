using UnityEngine;
using UnityEngine.UI;

public class GiantHealthBar : MonoBehaviour
{
    private GiantHealth GiantHealth;
    private Image giantHealthBar;

    [Header("Distance checks")]
    private Transform player;
    private GameObject healthBarCanvas;
    public float displayDistance = 20f;

    private void Start()
    {
        GiantHealth = GetComponent<GiantHealth>();

        if (GiantHealth != null)
            GiantHealth.OnHealthChanged += UpdateBar;
    }

    private void Update()
    {
        if (player == null || giantHealthBar == null)
            return;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= displayDistance)
            EnableHealthBar();
        else
            DisableHealthBar();
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
