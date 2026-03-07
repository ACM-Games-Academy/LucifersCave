using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponParent;
    [SerializeField] private WeaponStats startingWeapon;
    [SerializeField] private RecoilProfiles startingRecoil;
    private IWeaponFactory weaponFactory;
    private GameObject currentWeaponObject;
    public Initializer initializer;
    public WeaponStats currentWeapon { get; private set; }
    public RecoilProfiles currentRecoil { get; private set; }

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
            EquipWeapon(currentWeapon, currentRecoil, Vector3.zero);
        }
    }

    public void EquipWeapon(WeaponStats newWeapon, RecoilProfiles weaponRecoilStats, Vector3 weaponPosition)
    {
        if (newWeapon == null)
        {
            return;
        }
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, startingRecoil, weaponParent, spawnOffset);
        currentWeapon = newWeapon;
        currentWeaponObject.layer = LayerMask.NameToLayer("Hands/Weapon");


        var weaponRecoil = currentWeaponObject.GetComponent<WeaponRecoil>();
        if (weaponRecoil != null)
        {
            weaponRecoil.Initialize(initializer.recoilProfiles, initializer.playerCameraTransform);
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
    }
}

