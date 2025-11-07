using UnityEngine;

public interface IWeaponFactory
{
    GameObject CreateWeapon(WeaponStats weaponStats, Transform parent);
}

public class WeaponFactory : IWeaponFactory
{
    public GameObject CreateWeapon(WeaponStats weaponStats, Transform parent)
    {
        if (weaponStats.weaponPrefab == null)
        {
            Debug.LogError($"Weapon Prefab is not assigned for {weaponStats.weaponName}");
            return null;
        }

        GameObject weapon = Object.Instantiate(weaponStats.weaponPrefab, parent, false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.GetComponent<ShootScript>().ApplyWeaponData(weaponStats);
        return weapon;
    }
}
