using UnityEngine;

public class ResetStates : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Movement.canMove = true;
        CameraLook.canLook = true;
        PlayerHealth.isDead = false;

        Debug.Log("TimeScale: " + Time.timeScale);
    }
}
