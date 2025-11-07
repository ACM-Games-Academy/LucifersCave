using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponParent;
    [SerializeField] private WeaponStats startingWeapon;
    private IWeaponFactory weaponFactory;
    private GameObject currentWeaponObject;
    public Initializer initializer;
    public WeaponStats currentWeapon { get; private set; }

    private void Awake()
    {
        weaponFactory = new WeaponFactory();
    }

    public void Start()
    {
        if (startingWeapon != null)
        {
            currentWeapon = startingWeapon;
            EquipWeapon(currentWeapon);
        }
    }

    public void EquipWeapon(WeaponStats newWeapon)
    {
        if (newWeapon == null)
        {
            return;
        }
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, weaponParent);
        currentWeapon = newWeapon;
        Debug.Log($"Equipping weapon: {newWeapon.weaponName} | Prefab: {newWeapon.weaponPrefab}");

        var shootScript = currentWeaponObject.GetComponent<ShootScript>();
        if (shootScript != null)
        {
            shootScript.Initialize(
                initializer.movement,
                initializer.playerScore,
                initializer.muzzleFlash,
                initializer.playerCamera,
                initializer.weaponRecoil
            );
        }

        var reloading = currentWeaponObject.GetComponent<Reloading>();
        if (reloading != null)
        {
            reloading.Initialize(initializer.ammoCounter, initializer.reloadingText);
        }
    }
}

