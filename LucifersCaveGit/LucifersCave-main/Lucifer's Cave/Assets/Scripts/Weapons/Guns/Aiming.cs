using UnityEngine;

public class Aiming : MonoBehaviour
{
    [Header("Settings")]
    public float sensDecrease = 2f;
    public float aimMultiplier = 2f;
    public Camera mainCam;
    public Camera gunCam;
    public float FOV_increase;
    public float gunFOV_increase;
    public float FOV_decrease;
    public float FOV_speed;
    private float targetFOV;
    private float gunFOV;

    [Header("References")]
    public WeaponStats weaponStats;
    public ShootScript shootingFunc;
    public Movement movementFunc;
    public CameraLook lookFunc;
    public WeaponSway weaponSway;

    [Header("ADS Targeting")]
    private float defaultSpread;
    private float defaultSensitivity;
    private bool wasAiming = false;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        defaultSpread = weaponStats.spreadIntensity;
        defaultSensitivity = lookFunc.sensitivityAmount;

        mainCam = GetComponentInParent<Camera>();
        weaponSway = GetComponentInParent<WeaponSway>();
        animator = GetComponentInParent<Animator>();
        shootingFunc = GetComponentInChildren<ShootScript>();

        targetFOV = FOV_decrease;
        gunFOV = FOV_decrease;
    }

    void LateUpdate()
    {
        if (shootingFunc.isAiming)
        {
            if (!wasAiming) EnterADS();
            AimDownSight();
        }
        else
        {
            if (wasAiming) ExitADS();
            ReturnPosition();
        }

        mainCam.fieldOfView = Mathf.Lerp(
            mainCam.fieldOfView,
            targetFOV,
            FOV_speed * Time.deltaTime);

        gunCam.fieldOfView = Mathf.Lerp(
            gunCam.fieldOfView,
            gunFOV,
            FOV_speed * Time.deltaTime);
    }

    private void AimDownSight()
    {
        animator.SetTrigger("AimDownSight");
    }

    public void ReturnPosition()
    {
        animator.SetTrigger("HipFire");
    }

    private void EnterADS()
    {
        weaponSway.enabled = false;
        weaponStats.spreadIntensity = defaultSpread / aimMultiplier;
        lookFunc.sensitivityAmount = defaultSensitivity / sensDecrease;
        movementFunc.canSprint = false;
        wasAiming = true;
        targetFOV = FOV_increase;
        gunFOV = gunFOV_increase;
    }

    private void ExitADS()
    {
        weaponSway.enabled = true;
        weaponStats.spreadIntensity = defaultSpread;
        lookFunc.sensitivityAmount = defaultSensitivity;
        movementFunc.canSprint = true;
        wasAiming = false;
        targetFOV = FOV_decrease;
        gunFOV = FOV_decrease;
    }

    public void Initialize(Movement move,
        CameraLook lookScript,
        Camera playerCamera,
        Camera fpsCam,
        WeaponStats weaponStats)
    {
        this.movementFunc = move;
        this.lookFunc = lookScript;
        this.mainCam = playerCamera;
        this.gunCam = fpsCam;
        this.weaponStats = weaponStats;

        defaultSpread = weaponStats.spreadIntensity;
        defaultSensitivity = lookFunc.sensitivityAmount;
    }
}

