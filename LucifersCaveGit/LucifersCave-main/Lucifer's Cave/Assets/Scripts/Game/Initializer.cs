using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Initializer : MonoBehaviour
{
    [Header("Scripts")]
    public WeaponRecoil weaponRecoil;
    public WeaponStats weaponStats;
    public RecoilProfiles recoilProfiles;
    public ShootScript shootScript;
    public Movement movement;
    public Aiming aiming;
    public CameraLook cameraLook;
    public Reloading reloading;
    public PlayerScore playerScore;

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
        ShootScript[] shootScripts = Object.FindObjectsByType<ShootScript>(FindObjectsSortMode.None);
        Reloading[] reloadingScripts = Object.FindObjectsByType<Reloading>(FindObjectsSortMode.None);

        foreach (ShootScript shootScript in shootScripts)
        {
            shootScript.Initialize(movement, playerScore, muzzleFlash, playerCamera, weaponRecoil);
        }

        foreach (Reloading reloading in reloadingScripts)
        {
            reloading.Initialize(ammoCounter, reloadingText);
        }

        foreach (EnemyAttack enemyAttack in enemyAttackScripts)
        {
            enemyAttack.Initialize(player);
        }

        foreach (EnemyBase enemyBase in enemyBaseScripts)
        {
            enemyBase.Initialize(player);
        }

        weaponRecoil.Initialize(recoilProfiles, playerCameraTransform);
        aiming.Initialize(movement, cameraLook, playerCamera, fpsCamera, weaponStats, crosshair);
    }
}
