using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    [Header("Scripts")]
    public WeaponRecoil weaponRecoil;
    public WeaponStats weaponStats;
    public RecoilProfiles recoilProfiles;
    public Movement movement;
    public Aiming aiming;
    public CameraLook cameraLook;
    public PlayerScore playerScore;
    public Grenade grenadeScript;
    public WallBuy wallBuyScript;
    public WeaponManager weaponManager;

    [Header("References")]
    public ParticleSystem muzzleFlash;
    public Camera playerCamera;
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadingText;
    public Camera fpsCamera;
    public Transform player;
    public Transform playerCameraTransform;
    public Image crosshair;

    void Start()
    {
        EnemyAttack[] enemyAttackScripts = Object.FindObjectsByType<EnemyAttack>(FindObjectsSortMode.None);
        EnemyBase[] enemyBaseScripts = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        Grenade[] grenadeScripts = Object.FindObjectsByType<Grenade>(FindObjectsSortMode.None);
        WallBuy[] wallBuyScripts = Object.FindObjectsByType<WallBuy>(FindObjectsSortMode.None);

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
            wallBuy.Initialize(weaponManager, playerScore);
        }

        weaponRecoil.Initialize(recoilProfiles, playerCameraTransform);
        aiming.Initialize(movement, cameraLook, playerCamera, fpsCamera, weaponStats, crosshair);
    }
}
