using TMPro;
using UnityEngine;

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

    [Header("References")]
    public ParticleSystem muzzleFlash;
    public Camera playerCamera;
    public TextMeshProUGUI ammoCounter;
    public Camera fpsCamera;
    public Transform player;
    public Transform playerCameraTransform;


    void Start()
    {
        EnemyAttack[] enemyAttackScripts = Object.FindObjectsByType<EnemyAttack>(FindObjectsSortMode.None);
        EnemyBase[] enemyBaseScripts = Object.FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        ShootScript[] shootScripts = Object.FindObjectsByType<ShootScript>(FindObjectsSortMode.None);
        Reloading[] reloadingScripts = Object.FindObjectsByType<Reloading>(FindObjectsSortMode.None);

        foreach (ShootScript shootScript in shootScripts)
        {
            shootScript.Initialize(movement, muzzleFlash, playerCamera, weaponRecoil);
        }

        foreach (Reloading reloading in reloadingScripts)
        {
            reloading.Initialize(ammoCounter);
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
        aiming.Initialize(movement, cameraLook, playerCamera, fpsCamera, weaponStats);
    }
}
