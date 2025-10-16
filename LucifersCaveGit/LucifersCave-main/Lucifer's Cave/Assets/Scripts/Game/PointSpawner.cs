using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    public Transform spawnPosition;

    public void ShowPoints(int amount)
    {
        if (floatingTextPrefab)
        {
            GameObject obj = Instantiate(floatingTextPrefab, spawnPosition.position, Quaternion.identity, transform.parent);
            obj.GetComponent<FloatingText>().Init("+" + amount, spawnPosition.position, spawnPosition.position);
        }
    }
}