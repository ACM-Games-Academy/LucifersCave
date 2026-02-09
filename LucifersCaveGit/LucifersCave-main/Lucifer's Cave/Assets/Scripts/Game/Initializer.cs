using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    [Header("Scripts")]
    public WeaponStats weaponStats;
    public RecoilProfiles recoilProfiles;
    public Movement movement;
    public CameraLook cameraLook;
    public PlayerScore playerScore;
    public Grenade grenadeScript;
    public WeaponManager weaponManager;
    public PointSpawner pointSpawner;
    public Reloading reloadingScript;

    [Header("References")]
    public ParticleSystem muzzleFlash;
    public Camera playerCamera;
    public Camera fpsCamera;
    private bool bound;

    [Header("Transforms")]
    public Transform player;
    public Transform playerCameraTransform;
    public Transform rightHandTransform;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI vialCounter;
    public Image crosshair;
    public Image healthBar;

    private void Awake()
    {
        reloadingScript = cameraLook.GetComponent<Reloading>();
        BindAll();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!bound)
        {
            BindAll();
        }
    }

    private void BindAll()
    {
        EnemyAttack[] enemyAttackScripts = Object.FindObjectsByType<EnemyAttack>(FindObjectsSortMode.None);
        EnemyBase[] enemyBaseScripts = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        Grenade[] grenadeScripts = Object.FindObjectsByType<Grenade>(FindObjectsSortMode.None);
        WallBuy[] wallBuyScripts = Object.FindObjectsByType<WallBuy>(FindObjectsSortMode.None);
        GiantHealthBar[] giantHealthBarScripts = Object.FindObjectsByType<GiantHealthBar>(FindObjectsSortMode.None);
        InteractVial[] interactVialScripts = Object.FindObjectsByType<InteractVial>(FindObjectsSortMode.None);
        AmmoCache[] ammoCacheScripts = Object.FindObjectsByType<AmmoCache>(FindObjectsSortMode.None);

        foreach (AmmoCache ammoCache in ammoCacheScripts)
        {
            ammoCache.Initialize(reloadingScript);
        }

        foreach (InteractVial interactVial in interactVialScripts)
        {
            interactVial.Initialize(vialCounter);
        }

        foreach (GiantHealthBar giantHealthBar in giantHealthBarScripts)
        {
            giantHealthBar.Initialize(healthBar);
        }

        foreach (EnemyAttack enemyAttack in enemyAttackScripts)
        {
            enemyAttack.Initialize(player);
        }

        foreach (EnemyBase enemyBase in enemyBaseScripts)
        {
            enemyBase.Initialize(player);
        }

        foreach (Grenade grenade in grenadeScripts)
        {
            grenade.Initialize(playerScore);
        }

        foreach (WallBuy wallBuy in wallBuyScripts)
        {
            wallBuy.Initialize(weaponManager, playerScore, pointSpawner);
        }
    }
}
