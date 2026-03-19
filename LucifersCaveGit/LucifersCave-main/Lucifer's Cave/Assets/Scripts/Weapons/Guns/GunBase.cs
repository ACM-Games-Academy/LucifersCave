using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Reloading))]
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

    public int currentAmmo;
    public int reserveAmmo;
    public int maxAmmo;

    private int currentBurst;
    public bool isShooting, readyToShoot = true;
    public bool allowReset = true;

    public bool isAiming;

    public AudioSource audioSource;

    [Header("Reloading")]
    public bool isReloading;

    public void Start()
    {
        currentAmmo = weaponStats.maxAmmoInMag;
        reserveAmmo = weaponStats.ammoInReserve;
        maxAmmo = weaponStats.maxAmmoInMag;

        reloading = GetComponent<Reloading>();
        reloading.UpdateAmmo();
    }

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

        reloading.UpdateAmmo();
    }

    public IEnumerator Reload()
    {
        Reloading reloadComponent = GetComponent<Reloading>();

        if (isReloading || currentAmmo == maxAmmo || reserveAmmo <= 0)
            yield break;

        isReloading = true;

        reloadComponent.reloadingText.enabled = true;

        if (weaponSway != null)
            weaponSway.enabled = false;

        reloadComponent.animator.SetTrigger("ReloadDown");
        StartCoroutine(reloadComponent.PlayAfterAnimation("ReloadAnim", "ReloadUpAnim"));

        float reloadDuration = currentAmmo > 0 ? weaponStats.reloadTime : weaponStats.reloadTimeEmpty;
        yield return new WaitForSeconds(reloadDuration);

        weaponSway.enabled = true;

        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;

        reloadComponent.reloadingText.enabled = false;

        reloadComponent.UpdateAmmo();
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