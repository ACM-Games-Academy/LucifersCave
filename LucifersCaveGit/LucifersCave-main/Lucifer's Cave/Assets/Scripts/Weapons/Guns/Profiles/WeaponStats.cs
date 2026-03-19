using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [Header("Stats")]
    public float bulletVelocity;
    public GameObject bulletPrefab;
    public GameObject weaponPrefab;
    public string weaponName;

    [Header("Shooting")]
    public float bulletPrefabLifeTime;
    public float shootingDelay;
    public float spreadIntensity;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode shootingMode;

    [Header("Burst Mode")]
    public int bulletsPerBurst;

    [Header("Reloading")]
    public int currentAmmo;
    public int maxAmmoInMag;
    public int ammoInReserve;
    public float reloadTime;
    public float reloadTimeEmpty;

    [Header("Animations")]
    public RuntimeAnimatorController animatorController;
}