using System.Collections;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun
{
    [SerializeField] protected WeaponStats weaponStats;
    [SerializeField] protected RecoilProfiles recoilProfiles;
    [HideInInspector] public WeaponStats WeaponStats => weaponStats;
    [HideInInspector] public RecoilProfiles RecoilProfiles => recoilProfiles;

    public Movement movement;
    public WeaponSway weaponSway;
    public Reloading reloading;

    public Transform bulletSpawnPoint;
    public ParticleSystem muzzleFlash;
    public Camera playerCamera;

    private int currentBurst;
    public bool isShooting, readyToShoot = true;
    public bool allowReset = true;

    public static bool isAiming;
    public static bool isReloading;

    public abstract void Shoot();

    public abstract Vector3 CalculateSpread();

    public void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public void Initialize(WeaponContext weaponContext)
    {
        this.movement = weaponContext.movement;
        this.weaponSway = weaponContext.weaponSway;
        this.playerCamera = weaponContext.playerCamera;
    }
}

public class WeaponContext
{
    public Movement movement;
    public WeaponSway weaponSway;
    public Camera playerCamera;
}

public interface IGun
{
    void Shoot();
    Vector3 CalculateSpread();
    void ResetShot();
}

public interface IReloadable
{
    IEnumerator Reload();
}

public interface IAimable
{
    void Aim();
    void StopAiming();
}