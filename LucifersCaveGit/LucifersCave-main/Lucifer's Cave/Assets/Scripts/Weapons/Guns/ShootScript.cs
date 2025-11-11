using TMPro;
using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    public WeaponStats weaponData;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Camera plyrCamera;
    private int currentBurst;

    [Header("Input")]
    [SerializeField] private KeyCode aimingKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode shootingKey = KeyCode.Mouse0;

    [Header("Bools")]
    public bool isShooting, readyToShoot = true;
    [SerializeField] private bool allowReset = true;
    public bool isReloading;
    public bool isAiming;

    [Header("References")]
    public WeaponRecoil Recoil;
    public WeaponSway weaponSway;
    public Movement movementScript;
    public Reloading reloading;
    public PlayerScore playerScore;

    [Header("Reloading")]
    public Animator animator;

    [Header("Audio")]
    private AudioSource shootingSound;

    private bool isInitialized = false;

    public void Initialize(Movement movementScript,
        PlayerScore playerScore,
        ParticleSystem muzzleFlash,
        Camera playerCam,
        WeaponRecoil recoil)
    {
        this.movementScript = movementScript;
        this.playerScore = playerScore;
        this.muzzleFlash = muzzleFlash;
        plyrCamera = playerCam;
        Recoil = recoil;

        isInitialized = true;
    }

    public void ApplyWeaponData(WeaponStats data)
    {
        weaponData = data;
    }

    private void Start()
    {
        currentBurst = weaponData.bulletsPerBurst;

        weaponSway = GetComponentInParent<WeaponSway>();
        reloading = GetComponent<Reloading>();
        animator = GetComponentInParent<Animator>();
        shootingSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isInitialized || weaponData == null || reloading == null || movementScript == null)
        {
            return;
        }
        HandleInput();
    }

    private void HandleInput()
    {
        if (weaponData.shootingMode == WeaponStats.ShootingMode.Auto)
            isShooting = Input.GetKey(shootingKey) && reloading.currentAmmo > 0;
        else
            isShooting = Input.GetKeyDown(shootingKey) && reloading.currentAmmo > 0;

        if (readyToShoot && isShooting && !isReloading && !movementScript.isSprinting)
        {
            currentBurst = weaponData.bulletsPerBurst;
            Shoot();
            Recoil.Recoil(isAiming);
            Recoil.HandleRecoilAnimation();
        }

        bool aimKeyHeld = Input.GetKey(aimingKey);
        isAiming = aimKeyHeld && !reloading.isReloading && !movementScript.isSprinting;
    }

    void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        reloading.currentAmmo--;
        reloading.UpdateAmmo();
        shootingSound.Play();

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(weaponData.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * weaponData.bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, weaponData.bulletPrefabLifeTime));
        bullet.transform.forward = shootingDirection;

        Bullets bullets = bullet.GetComponent<Bullets>();
        bullets.Initialize(playerScore);

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

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
}