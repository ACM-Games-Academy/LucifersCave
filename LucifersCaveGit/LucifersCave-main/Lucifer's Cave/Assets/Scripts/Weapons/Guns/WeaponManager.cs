using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform weaponParent;
    [SerializeField] private WeaponStats startingWeapon;
    [SerializeField] private RecoilProfiles startingRecoil;
    private IWeaponFactory weaponFactory;
    private GameObject currentWeaponObject;
    public Initializer initializer;
    public WeaponStats currentWeapon { get; private set; }
    public RecoilProfiles currentRecoil { get; private set; }

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

        currentWeaponObject = weaponFactory.CreateWeapon(newWeapon, startingRecoil, weaponParent, Vector3.zero);
        currentWeapon = newWeapon;
        currentWeaponObject.layer = LayerMask.NameToLayer("Hands/Weapon");

        var weaponRecoil = currentWeaponObject.GetComponent<WeaponRecoil>();
        if (weaponRecoil != null)
        {
            weaponRecoil.Initialize(initializer.recoilProfiles, initializer.playerCameraTransform);
        }

        var shootScript = currentWeaponObject.GetComponent<ShootScript>();
        if (shootScript != null)
        {
            shootScript.Initialize(
                initializer.movement,
                initializer.playerScore,
                initializer.muzzleFlash,
                initializer.playerCamera,
                weaponRecoil
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

        var melee = initializer.rightHandTransform.GetComponent<Melee>();
        if (melee != null)
        {
            Transform attack = currentWeaponObject.transform.Find("attackPoint");
            if (attack != null)
            {
                melee.attackPoint = attack.gameObject;
                Debug.Log($"attackPoint assigned to Melee: {attack.name}");
            }
            else
            {
                Debug.LogWarning($"No 'attackPoint' found under {currentWeaponObject.name}");
            }
        }
    }
}

