using UnityEngine;

public class RoomCulling : MonoBehaviour
{
    private Transform player;
    private float checkInterval = 0.5f;
    private float timer;

    public float activeDistance = 50f;

    private bool isActive = true;

    private Renderer[] renderers;
    private Collider[] colliders;

    void Start()
    {
        player = Camera.main.transform;

        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < checkInterval) return;
        timer = 0f;

        float sqrDistance = Vector3.Distance(transform.position, player.position);
        float sqrActiveDistance = activeDistance * activeDistance;

        if (sqrDistance < sqrActiveDistance)
        {
            if (!isActive)
            {
                SetRoomActive(true);
            }
        }
        else
        {
            if (isActive)
            {
                SetRoomActive(false);
            }
        }
    }

    void SetRoomActive(bool state)
    {
        isActive = state;

        foreach (Renderer r in renderers)
        {
            r.enabled = state;
        }

        foreach (Collider c in colliders)
        {
            c.enabled = state;
        }
    }
}
