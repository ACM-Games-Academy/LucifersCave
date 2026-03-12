using System.Collections;
using UnityEngine;

public class Pistol : GunBase
{
    public void Start()
    {
        reloading = GetComponent<Reloading>();
        reloading.UpdateAmmo();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (readyToShoot && reloading.currentAmmo > 0)
            {
                Shoot();
            }
        } 
    }

    public override void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();
        reloading.currentAmmo--;
        reloading.UpdateAmmo();
        audioSource.Play();

        Vector3 shootingDirection = CalculateSpread().normalized;

        GameObject bullet = Instantiate(weaponStats.bulletPrefab, 
            bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * 
            weaponStats.bulletVelocity, ForceMode.VelocityChange);
        bullet.transform.forward = shootingDirection;

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