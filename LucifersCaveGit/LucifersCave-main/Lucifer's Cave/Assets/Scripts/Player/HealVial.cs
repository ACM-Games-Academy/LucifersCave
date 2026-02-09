using UnityEngine;
using System.Collections;
using TMPro;

public class HealVial : MonoBehaviour
{
    [Header("KeyBinds")]
    public KeyCode useKey = KeyCode.V;

    [Header("Carrot Settings")]
    public float carrotRestoreMultiplier;
    public ParticleSystem vialEffect;

    [Header("Audio")]
    public AudioSource healingAudioSource;
    public AudioClip healSound;

    [Header("References")]
    public TextMeshProUGUI vialCounter;
    private PlayerHealthBar playerHealth;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealthBar>();
    }

    public void Initialize(ParticleSystem vialEffect)
    {
        this.vialEffect = vialEffect;
    }

    void Update()
    {
        if (Input.GetKeyDown(useKey) && int.TryParse(vialCounter.text, out int vialCount)
            && vialCount > 0 && playerHealth.playerHealthBar.fillAmount < 1)
        {
            RestoreHealth();
            healingAudioSource.PlayOneShot(healSound);
        }
    }

    public void RestoreHealth()
    {
        StartCoroutine(UseVialAnimation());
    }

    public IEnumerator UseVialAnimation()
    {
        float startStamina = playerHealth.playerHealthBar.fillAmount;
        vialEffect.Play();

        float timeRunning = 0f;
        while (playerHealth.playerHealthBar.fillAmount < 1f && timeRunning < 1)
        {
            timeRunning += Time.deltaTime;
            float animationTime = timeRunning / 1f;
            playerHealth.playerHealthBar.fillAmount = Mathf.Lerp(startStamina, 1f, animationTime);

            yield return null;
        }
        playerHealth.playerHealthBar.fillAmount = 1f;

        vialEffect.Stop();
    }
}