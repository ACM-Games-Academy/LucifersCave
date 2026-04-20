using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AudioSource buttonAudioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [SerializeField] private GameObject confirmationScreen;

    public enum ButtonType
    {
        Start,
        Quit,
        ConfirmQuit,
        ReturnToMenu
    }

    public ButtonType buttonType;

    void Start()
    {
        confirmationScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TextMeshProUGUI currentTMPro = GetComponent<TextMeshProUGUI>();
        currentTMPro.color = Color.red;
        buttonAudioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI currentTMPro = GetComponent<TextMeshProUGUI>();
        currentTMPro.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Start:
                StartCoroutine(LoadSceneWithSound());
                break;
            case ButtonType.Quit:
                confirmationScreen.SetActive(true);
                buttonAudioSource.PlayOneShot(clickSound);
                break;
            case ButtonType.ConfirmQuit:
                Application.Quit();
                buttonAudioSource.PlayOneShot(clickSound);
                break;
            case ButtonType.ReturnToMenu:
                confirmationScreen.SetActive(false);
                buttonAudioSource.PlayOneShot(clickSound);
                break;
        }
    }

    private IEnumerator LoadSceneWithSound()
    {
        buttonAudioSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        SceneManager.LoadScene("Main_Level");
    }
}

