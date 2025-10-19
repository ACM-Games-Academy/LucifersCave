using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject confirmationScreen;

    public void Start()
    {
        confirmationScreen.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main_Level");
    }

    public void QuitGame()
    {
        OpenConfirmation();
    }

    public void OpenConfirmation()
    {
        confirmationScreen.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void ConfirmNo()
    {
        confirmationScreen.SetActive(false);
    }
}

