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
        yield return new WaitForSecondsRealtime(delay);
        float currentTime = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
            deathTitle.color = new Color(deathTitle.color.r, deathTitle.color.g, deathTitle.color.b, alpha);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        deathTitle.color = new Color(deathTitle.color.r, deathTitle.color.g, deathTitle.color.b, 1f);
    }

    public IEnumerator SlowDownTime(float timeTarget)
    {
        float currentTime = 0f;
        while (currentTime < fadeDuration)
        {
            Time.timeScale = Mathf.Lerp(1f, timeTarget, currentTime / fadeDuration);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = timeTarget;
    }
}