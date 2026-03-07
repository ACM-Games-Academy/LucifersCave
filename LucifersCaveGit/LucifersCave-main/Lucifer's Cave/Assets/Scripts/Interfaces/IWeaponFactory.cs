using UnityEngine;

public interface IWeaponFactory
{
    GameObject CreateWeapon(WeaponStats weaponStats, RecoilProfiles weaponRecoil, Transform parent, Vector3 weaponPosition);
}

public class WeaponFactory : IWeaponFactory
{
    public GameObject CreateWeapon(WeaponStats weaponStats, RecoilProfiles weaponRecoil, Transform parent, Vector3 weaponPosition)
    {
        if (weaponStats.weaponPrefab == null)
        {
            Debug.LogError($"Weapon Prefab is not assigned for {weaponStats.weaponName}");
            return null;
        }

        GameObject weapon = Object.Instantiate(weaponStats.weaponPrefab, parent, false);
        weapon.transform.localPosition += weaponPosition;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.GetComponent<GunBase>().ApplyWeaponData(weaponStats);
        weapon.GetComponent<WeaponRecoil>().ApplyRecoilData(weaponRecoil);
        weapon.GetComponentInParent<Melee>().SetKnife(weapon.transform.Find("attackPoint"));
        return weapon;
    }
}
