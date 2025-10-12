using System.Collections;
using TMPro;
using UnityEngine;

public class Reloading : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    public int currentAmmo, reserveAmmo, maxAmmo;

    [Header("Input")]
    public KeyCode reloadKey = KeyCode.R;

    [Header("References")]
    private ShootScript shootScript;
    public WeaponStats weaponStats;
    public WeaponSway weaponSway;

    public Animator animator;

    void Start()
    {
        currentAmmo = weaponStats.currentAmmo;
        reserveAmmo = weaponStats.reserveAmmo;
        maxAmmo = weaponStats.maxAmmo;

        UpdateAmmo();

        weaponSway = GetComponentInParent<WeaponSway>();
        shootScript = GetComponent<ShootScript>();
        animator = GetComponentInParent<Animator>();
    }

    public void ApplyWeaponReloadData(WeaponStats weaponStats)
    {
        this.currentAmmo = weaponStats.currentAmmo;
        this.reserveAmmo = weaponStats.reserveAmmo;
        this.maxAmmo = weaponStats.maxAmmo;
    }

    public void Initialize(TextMeshProUGUI ammoCounter)
    {
        this.ammoCounter = ammoCounter;
    }

    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(reloadKey) && currentAmmo < maxAmmo && reserveAmmo > 0)
            StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        if (shootScript.isReloading || currentAmmo == weaponStats.maxAmmo)
            yield break;

        shootScript.isReloading = true;
        shootScript.readyToShoot = false;
        weaponSway.enabled = false;

        animator.SetTrigger("ReloadDown");

        float reloadDuration = currentAmmo > 0 ? weaponStats.reloadTime : weaponStats.reloadTimeEmpty;
        yield return new WaitForSeconds(reloadDuration);

        animator.SetTrigger("ReloadUp");

        weaponSway.enabled = true;

        int ammoToReload = Mathf.Min(weaponStats.maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        shootScript.isReloading = false;
        shootScript.readyToShoot = true;

        weaponStats.currentAmmo = currentAmmo;

        UpdateAmmo();
    }

    public void UpdateAmmo()
    {
        if (ammoCounter != null)
        {
            ammoCounter.text = currentAmmo + " / " + reserveAmmo;
        }
    }
}
