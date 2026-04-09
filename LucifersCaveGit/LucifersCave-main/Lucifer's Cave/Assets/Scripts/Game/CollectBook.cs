using UnityEngine;

public class CollectBook : MonoBehaviour, IInteractable
{
    public bool finalPhaseGame;
    public GameObject finalBook;
    public GameObject bossFightHUD;
    public GameObject enemyBoss;

    public Transform bossSpawnPosition;
    bool hasInteracted;

    void Start()
    {
        finalBook.SetActive(true);
    }

    public void Interact()
    {
        if (hasInteracted) return;
        hasInteracted = true;

        finalPhaseGame = true;
        finalBook.SetActive(false);
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (finalPhaseGame)
        {
            Instantiate(enemyBoss, bossSpawnPosition.position, Quaternion.identity);

            bossFightHUD.SetActive(true);
        }
    }
}
