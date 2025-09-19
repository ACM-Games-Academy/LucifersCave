using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform camPos;

    void Update()
    {
        transform.position = camPos.position;
    }
}
