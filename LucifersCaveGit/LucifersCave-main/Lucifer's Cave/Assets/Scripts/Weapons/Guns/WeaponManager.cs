using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponParent;
    [SerializeField] private WeaponStats startingWeapon;
    [SerializeField] private RecoilProfiles startingRecoil;
    public GunBase gunBase;
    private IWeaponFactory weaponFactory;
    private GameObject currentWeaponObject;
    public Initializer initializer;

    public WeaponStats currentWeapon { get; private set; }
    public RecoilProfiles currentRecoil { get; private set; }
    public Reloading currentReloading { get; private set; }

    public Vector3 spawnOffset;

    private void Awake()
    {
        weaponFactory = new WeaponFactory();
    }

    public void Start()
    {
        if (startingWeapon != null)
        {
            currentWeapon = startingWeapon;
            currentRecoil = startingRecoil;

            EquipWeapon(currentWeapon, currentRecoil, Vector3.zero);
        }
    }

    public void EquipWeapon(WeaponStats newWeapon, RecoilProfiles weaponRecoilStats, Vector3 weaponPosition)
    {
        if (newWeapon == null) return;

        if (currentWeaponObject != null)
            Destroy(currentWeaponObject);

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, weaponRecoilStats, weaponParent, spawnOffset);

        gunBase = currentWeaponObject.GetComponent<GunBase>();

        currentReloading = currentWeaponObject.GetComponent<Reloading>();
        currentWeapon = newWeapon;
        currentRecoil = weaponRecoilStats;
        currentWeaponObject.layer = LayerMask.NameToLayer("Hands/Weapon");


        var weaponRecoil = currentWeaponObject.GetComponent<WeaponRecoil>();
        if (weaponRecoil != null && currentWeaponObject != null)
        {
            weaponRecoil.Initialize(weaponRecoilStats, initializer.playerCameraTransform);
        }

        var shootScript = currentWeaponObject.GetComponent<GunBase>();
        if (shootScript != null)
        {
            shootScript.Initialize(
                weaponContext: new WeaponContext
                {
                    movement = initializer.movement,
                    weaponSway = initializer.weaponSway,
                    playerCamera = initializer.playerCamera
                }
            );
        }

        var reloading = currentWeaponObject.GetComponent<Reloading>();
        if (reloading != null)
        {
            reloading.Initialize(initializer.ammoCounter, initializer.reloadingText);
        }

        var aimingScript = currentWeaponObject.GetComponent<Aiming>();
        if (aimingScript != null)
        {
            aimingScript.Initialize(
                initializer.movement,
                initializer.cameraLook,
                initializer.playerCamera,
                initializer.fpsCamera,
                newWeapon,
                initializer.crosshair
            );
        }

        if (initializer == null)
        {
            Debug.LogError("Initializer missing!");
            return;
        }

        if (initializer.playerCameraTransform == null)
        {
            Debug.LogError("playerCameraTransform not assigned in Initializer!");
        }
    }
}

