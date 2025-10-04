using UnityEngine;

public class WeaponCamera : MonoBehaviour
{
    public Camera mainCamera;

    void LateUpdate()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
