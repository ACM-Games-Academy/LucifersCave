using UnityEngine;
using UnityEngine.UI;

public class Aiming : MonoBehaviour, IAimable
{
    [Header("Settings")]
    public float adsSensitivityMultiplier = 2f;
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
    public Movement movementFunc;
    public CameraLook lookFunc;
    public WeaponSway weaponSway;
    public GunBase gunBase;

    [Header("ADS Targeting")]
    private float defaultSpread;
    private float defaultSensitivity;
    private bool wasAiming = false;
    public Image crosshair;

    [Header("Animation")]
    public Animator animator;
    public bool isAnimating;

    private bool isInitialized = false;

    void LateUpdate()
    {
        if (!isInitialized)
        {
            return;
        }

        if (gunBase.isAiming && !gunBase.isReloading)
        {
            if (!wasAiming)
            {
                EnterADS();
                wasAiming = true;
            }
            
            animator.SetBool("isAimingAnim", true);
        }
        else
        {
            if (wasAiming)
            {
                ExitADS();
                wasAiming = false;
            }

            animator.SetBool("isAimingAnim", false);
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

    public void EnterADS()
    {
        weaponSway.ResetSwayPosition();
        weaponSway.enabled = false;

        weaponStats.spreadIntensity = defaultSpread * aimMultiplier;
        lookFunc.sensitivityAmount = defaultSensitivity / adsSensitivityMultiplier;
        movementFunc.canSprint = false;
        targetFOV = FOV_increase;
        gunFOV = gunFOV_increase;
        crosshair.enabled = false;
    }

    public void ExitADS()
    {
        weaponSway.enabled = true;
        weaponStats.spreadIntensity = defaultSpread;
        lookFunc.sensitivityAmount = defaultSensitivity;
        movementFunc.canSprint = true;
        targetFOV = FOV_decrease;
        gunFOV = FOV_decrease;

        crosshair.enabled = true;
    }

    public void Initialize(Movement move,
        CameraLook lookScript,
        Camera playerCamera,
        Camera fpsCam,
        WeaponStats weaponStats,
        Image crosshair)
    {
        this.movementFunc = move;
        this.lookFunc = lookScript;
        this.mainCam = playerCamera;
        this.gunCam = fpsCam;
        this.weaponStats = weaponStats;
        this.crosshair = crosshair;

        weaponSway = GetComponentInParent<WeaponSway>();
        animator = GetComponentInParent<Animator>();

        targetFOV = FOV_decrease;
        gunFOV = FOV_decrease;

        defaultSpread = weaponStats.spreadIntensity;
        defaultSensitivity = lookFunc.sensitivityAmount;
        gunBase = GetComponent<GunBase>();

        isInitialized = true;
    }
}