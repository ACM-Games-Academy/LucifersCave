using System.Collections;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] GameObject[] HUD_Objects;
    [SerializeField] private TextMeshProUGUI deathTitle;
    float fadeDuration = 1f;

    private void Start()
    {
        deathScreen.SetActive(false);
    }

    public IEnumerator ShowDeathScreen(float delay)
    {
        foreach (var obj in HUD_Objects) {
            obj.SetActive(false);
        }

        deathScreen.SetActive(true);
        yield return new WaitForSeconds(delay);
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            deathTitle.color = new Color(deathTitle.color.r, deathTitle.color.g, deathTitle.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
