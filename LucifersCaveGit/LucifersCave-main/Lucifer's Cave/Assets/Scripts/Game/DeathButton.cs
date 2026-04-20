using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private TextMeshProUGUI buttonText;

    public AudioSource buttonAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public void Start()
    {
        buttonText = GetComponent<TextMeshProUGUI>();
        if (buttonAudioSource == null)
        {
            Debug.LogWarning("Button AudioSource is not assigned. Please assign it in the inspector.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonAudioSource.PlayOneShot(hoverSound);
        if (buttonText != null)
        {
            buttonText.color = Color.red;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = Color.white;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonAudioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("Main_Menu");
    }
}

