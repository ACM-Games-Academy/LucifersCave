using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class AutomaticRifle : GunBase
{
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (readyToShoot && currentAmmo > 0)
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isAiming = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isAiming = false;
        }

        if (PauseMenu.isPaused) return;
    }

    public override void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        currentAmmo--;
        reloading.UpdateAmmo();
        audioSource.Play();
        weaponRecoil.Recoil(isAiming);
        StartCoroutine(cameraShake.CameraShakeProcess(shakeDuration, shakeAmount));

        Vector3 shootingDirection = CalculateSpread().normalized;
        PlayerScore playerScore = FindFirstObjectByType<PlayerScore>();

        GameObject bullet = Instantiate(weaponStats.bulletPrefab,
            bulletSpawnPoint.position, Quaternion.LookRotation(shootingDirection));
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection *
            weaponStats.bulletVelocity, ForceMode.VelocityChange);
        bullet.transform.forward = shootingDirection;

        Bullets bullets = bullet.GetComponent<Bullets>();
        bullets.Initialize(playerScore);

        if (allowReset)
        {
            Invoke(nameof(ResetShot), weaponStats.shootingDelay);
            allowReset = false;
        }
    }

    public override Vector3 CalculateSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
        float x = Random.Range(-weaponStats.spreadIntensity, weaponStats.spreadIntensity);
        float y = Random.Range(-weaponStats.spreadIntensity, weaponStats.spreadIntensity);

        Vector3 spread = playerCamera.transform.right * x
            + playerCamera.transform.up * y;

        Vector3 finalDirection = (direction + spread).normalized;

        return finalDirection;
    }
}
