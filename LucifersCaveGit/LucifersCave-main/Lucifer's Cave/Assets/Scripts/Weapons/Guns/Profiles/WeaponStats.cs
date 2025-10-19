using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Scriptable Objects/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    [Header("Stats")]
    public float bulletVelocity;
    public GameObject bulletPrefab;

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
    public int maxAmmo;
    public int reserveAmmo;
    public float reloadTime;
    public float reloadTimeEmpty;

    [Header("Animations")]
    public RuntimeAnimatorController animatorController;
}