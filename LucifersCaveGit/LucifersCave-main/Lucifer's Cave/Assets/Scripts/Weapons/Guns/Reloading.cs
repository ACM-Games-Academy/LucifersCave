using System.Collections;
using TMPro;
using UnityEngine;

public class Reloading : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadingText;
    public int currentAmmo, reserveAmmo, maxAmmo;

    [Header("Input")]
    public KeyCode reloadKey = KeyCode.R;

    [Header("References")]
    public WeaponStats weaponStats;
    public WeaponSway weaponSway;
    private GunBase gunBase;

    public Animator animator;

    void Start()
    {
        currentAmmo = weaponStats.currentAmmo;
        reserveAmmo = weaponStats.reserveAmmo;
        maxAmmo = weaponStats.maxAmmo;

        UpdateAmmo();

        weaponSway = GetComponentInParent<WeaponSway>();
        animator = GetComponentInParent<Animator>();

        gunBase.isReloading = false;
    }

    public void ApplyWeaponReloadData(WeaponStats weaponStats)
    {
        this.currentAmmo = weaponStats.currentAmmo;
        this.reserveAmmo = weaponStats.reserveAmmo;
        this.maxAmmo = weaponStats.maxAmmo;
    }

    public void Initialize(TextMeshProUGUI ammoCounter, TextMeshProUGUI reloadingText)
    {
        this.ammoCounter = ammoCounter;
        this.reloadingText = reloadingText;
        reloadingText.enabled = false;

        gunBase = GetComponent<GunBase>();
    }

    void Update()
    {
        HandleInput();

        if (gunBase.isReloading)
        {
            animator.SetBool("isAimingAnim", false);
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(reloadKey) && currentAmmo < maxAmmo && reserveAmmo > 0 
            && !gunBase.isReloading && !gunBase.isAiming)
            StartCoroutine(gunBase.Reload());
    }

    public void UpdateAmmo()
    {
        if (ammoCounter != null)
        {
            ammoCounter.text = currentAmmo + " / " + reserveAmmo;
        }
    }

    public IEnumerator PlayAfterAnimation(string firstAnimName, string NextAnimName)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        while (!info.IsName(firstAnimName))
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        while (info.normalizedTime < 1.0f)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        animator.Play(NextAnimName);
    }
}