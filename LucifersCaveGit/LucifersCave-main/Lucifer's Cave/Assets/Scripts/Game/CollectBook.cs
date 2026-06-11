using UnityEngine;

public class CollectBook : MonoBehaviour, IInteractable
{
    public bool finalPhaseGame;
    public GameObject bossFightHUD;
    public GameObject enemyBoss;

    public Transform bossSpawnPosition;
    public Transform Player;
    bool hasInteracted;

    public Initializer initializer;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Interact()
    {
        if (hasInteracted) return;
        
        hasInteracted = true;
        finalPhaseGame = true;

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        SpawnBoss();
        Destroy(gameObject, 5f);
    }

    private void SpawnBoss()
    {
        if (finalPhaseGame)
        {
            GameObject instantiatedBoss = Instantiate(enemyBoss, bossSpawnPosition.position, Quaternion.identity);

            bossFightHUD.SetActive(true);

            BossBase bossBase = instantiatedBoss.GetComponent<BossBase>();
            bossBase.Initialize(Player);
            GiantHealthBar healthBar = instantiatedBoss.GetComponent<GiantHealthBar>();
            healthBar.Initialize(initializer.healthBar, Player, bossFightHUD);
        }
    }

    public void Initialize(GameObject bossFightHUD, GameObject enemyBoss, Transform bossSpawnPosition)
    {
        this.bossFightHUD = bossFightHUD;
        this.enemyBoss = enemyBoss;
        this.bossSpawnPosition = bossSpawnPosition;
    }
}
