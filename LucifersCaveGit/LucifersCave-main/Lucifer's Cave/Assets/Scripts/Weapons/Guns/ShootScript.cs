using TMPro;
using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    public WeaponStats weaponData;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private Camera plyrCamera;
    private int currentAmmo, reserveAmmo, currentBurst;

    [Header("Input")]
    [SerializeField] private KeyCode aimingKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode shootingKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    [Header("Bools")]
    [SerializeField] private bool isShooting, readyToShoot = true;
    [SerializeField] private bool allowReset = true;
    private bool isReloading;
    public bool isAiming;

    [Header("References")]
    public WeaponRecoil Recoil;
    public WeaponSway weaponSway;
    public Movement movementScript;

    [Header("Reloading")]
    public Vector3 reloadPosition;
    private Vector3 defaultPosition;
    public Quaternion reloadRotation;
    private Quaternion defaultRotation;
    public float ReloadAnimSpeed;

    public void Initialize(Movement movementScript,
        ParticleSystem muzzleFlash,
        TextMeshProUGUI ammoCounter,
        Camera playerCam,
        WeaponRecoil recoil)
    {
        this.movementScript = movementScript;
        this.muzzleFlash = muzzleFlash;
        this.ammoCounter = ammoCounter;
        this.plyrCamera = playerCam;
        Recoil = recoil;
    }

    public void ApplyWeaponData(WeaponStats data)
    {
        this.weaponData = data;
        this.currentAmmo = data.currentAmmo;
        this.reserveAmmo = data.reserveAmmo;
    }

    private void Start()
    {
        currentAmmo = weaponData.maxAmmo;
        reserveAmmo = weaponData.reserveAmmo;
        currentBurst = weaponData.bulletsPerBurst;
        UpdateAmmo();

        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;

        weaponSway = GetComponentInParent<WeaponSway>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAmmo();
    }

    private void HandleInput()
    {
        if (weaponData.shootingMode == WeaponStats.ShootingMode.Auto)
            isShooting = Input.GetKey(shootingKey) && currentAmmo > 0;
        else
            isShooting = Input.GetKeyDown(shootingKey) && currentAmmo > 0;

        if (readyToShoot && isShooting && !isReloading && !movementScript.isSprinting)
        {
            currentBurst = weaponData.bulletsPerBurst;
            Shoot();
            Recoil.Recoil(isAiming);
            Recoil.HandleRecoilAnimation();
        }

        if (Input.GetKeyDown(reloadKey) && currentAmmo < weaponData.maxAmmo && reserveAmmo > 0)
            StartCoroutine(Reload());

        isAiming = Input.GetKey(aimingKey);
    }

    void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        currentAmmo--;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * weaponData.bulletVelocity, ForceMode.Force);
        StartCoroutine(DestroyBulletAfterTime(bullet, weaponData.bulletPrefabLifeTime));
        bullet.transform.forward = shootingDirection;

        if (allowReset)
        {
            Invoke("ResetShot", weaponData.shootingDelay);
            allowReset = false;
        }

        if (weaponData.shootingMode == WeaponStats.ShootingMode.Burst && currentBurst > 1)
        {
            currentBurst--;
            Invoke("Shoot", weaponData.shootingDelay);
        }
    }

    private IEnumerator spawnTrail(TrailRenderer Trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = hit.point;

        Destroy(Trail.gameObject, Trail.time);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = plyrCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = (targetPoint - bulletSpawnPoint.position).normalized;
        float x = Random.Range(-weaponData.spreadIntensity, weaponData.spreadIntensity);
        float y = Random.Range(-weaponData.spreadIntensity, weaponData.spreadIntensity);

        Vector3 spread = plyrCamera.transform.right * x + plyrCamera.transform.up * y;

        Vector3 finalDirection = (direction + spread).normalized;

        return finalDirection;
    }

    public void UpdateAmmo()
    {
        if (ammoCounter != null)
        {
            ammoCounter.text = currentAmmo + " / " + reserveAmmo;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator Reload()
    {
        if (isReloading || currentAmmo == weaponData.maxAmmo)
            yield break;

        isReloading = true;
        readyToShoot = false;
        weaponSway.enabled = false;

        float reloadTime = 0f;
        while (reloadTime < 1f)
        {
            reloadTime += Time.deltaTime * ReloadAnimSpeed;
            transform.localPosition = Vector3.Lerp(transform.localPosition, reloadPosition, reloadTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, reloadRotation, reloadTime);
            yield return null;
        }

        yield return new WaitForSeconds(currentAmmo > 0 ? weaponData.reloadTime : weaponData.reloadTimeEmpty);

        reloadTime = 0f;
        while (reloadTime < 1f)
        {
            reloadTime += Time.deltaTime * ReloadAnimSpeed;
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, reloadTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultRotation, reloadTime);
            yield return null;
        }

        weaponSway.enabled = true;

        int ammoToReload = Mathf.Min(weaponData.maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;
        readyToShoot = true;
        UpdateAmmo();
    }
}
