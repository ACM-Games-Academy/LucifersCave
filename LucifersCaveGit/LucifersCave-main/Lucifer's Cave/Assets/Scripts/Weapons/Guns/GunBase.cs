using System.Collections;
using UnityEngine;

public abstract class GunBase : MonoBehaviour, IGun, IReloadable
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

    public bool isAiming;

    public AudioSource audioSource;

    [Header("Reloading")]
    public int currentAmmo, reserveAmmo, maxAmmo;
    public bool isReloading;

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

    public void ApplyWeaponData(WeaponStats weaponStats)
    {
        this.weaponStats = weaponStats;
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
        if (reserveAmmo > maxAmmo)
        {
            reserveAmmo = maxAmmo;
        }
    }

    public IEnumerator Reload()
    {
        if (isReloading || currentAmmo == weaponStats.maxAmmo)
            yield break;

        isReloading = true;

        reloading.reloadingText.enabled = true;
        isReloading = true;
        weaponSway.enabled = false;

        reloading.animator.SetTrigger("ReloadDown");
        StartCoroutine(reloading.PlayAfterAnimation("ReloadAnim", "ReloadUpAnim"));

        float reloadDuration = currentAmmo > 0 ? weaponStats.reloadTime : weaponStats.reloadTimeEmpty;
        yield return new WaitForSeconds(reloadDuration);

        weaponSway.enabled = true;

        int ammoToReload = Mathf.Min(weaponStats.maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;

        weaponStats.currentAmmo = currentAmmo;
        reloading.reloadingText.enabled = false;

        reloading.UpdateAmmo();
        isReloading = false;
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
    void EnterADS();
    void ExitADS();
}