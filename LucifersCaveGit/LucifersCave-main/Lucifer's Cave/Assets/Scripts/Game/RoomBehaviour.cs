using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < 4; i++)
        {
            walls[i].SetActive(status[i]);
            doors[i].SetActive(!status[i]);
        }
    }
}