using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;

    public void UpdateRoom(bool[] status)
    {
        if (walls.Length != 4 || doors.Length != 4 || status.Length != 4)
        {
            Debug.LogError("Room setup is incorrect! Must have exactly 4 walls and 4 doors.", this);
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}