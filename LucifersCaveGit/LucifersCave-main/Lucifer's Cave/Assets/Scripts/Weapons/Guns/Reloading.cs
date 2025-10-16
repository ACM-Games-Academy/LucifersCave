using System.Collections;
using TMPro;
using UnityEngine;

public class Reloading : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI reloadingText;
    public int currentAmmo, reserveAmmo, maxAmmo;
    public bool isReloading;

    [Header("Input")]
    public KeyCode reloadKey = KeyCode.R;

    [Header("References")]
    private ShootScript shootScript;
    public WeaponStats weaponStats;
    public WeaponSway weaponSway;

    public Animator animator;

    void Start()
    {
        currentAmmo = weaponStats.currentAmmo;
        reserveAmmo = weaponStats.reserveAmmo;
        maxAmmo = weaponStats.maxAmmo;

        UpdateAmmo();

        weaponSway = GetComponentInParent<WeaponSway>();
        shootScript = GetComponent<ShootScript>();
        animator = GetComponentInParent<Animator>();

        reloadingText.enabled = false;
        isReloading = false;
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
    }

    void Update()
    {
        HandleInput();

        if (isReloading)
        {
            animator.SetBool("isAimingAnim", false);
        }
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(reloadKey) && currentAmmo < maxAmmo && reserveAmmo > 0 && !shootScript.isAiming)
            StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        if (shootScript.isReloading || currentAmmo == weaponStats.maxAmmo)
            yield break;

        isReloading = true;

        reloadingText.enabled = true;
        shootScript.isReloading = true;
        shootScript.readyToShoot = false;
        weaponSway.enabled = false;

        animator.SetTrigger("ReloadDown");
        StartCoroutine(PlayAfterAnimation("ReloadAnim", "ReloadUpAnim"));

        float reloadDuration = currentAmmo > 0 ? weaponStats.reloadTime : weaponStats.reloadTimeEmpty;
        yield return new WaitForSeconds(reloadDuration);


        weaponSway.enabled = true;

        int ammoToReload = Mathf.Min(weaponStats.maxAmmo - currentAmmo, reserveAmmo);
        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        shootScript.isReloading = false;
        shootScript.readyToShoot = true;

        weaponStats.currentAmmo = currentAmmo;
        reloadingText.enabled = false;

        UpdateAmmo();
        isReloading = false;
    }

    public void UpdateAmmo()
    {
        if (ammoCounter != null)
        {
            ammoCounter.text = currentAmmo + " / " + reserveAmmo;
        }
    }

    private IEnumerator PlayAfterAnimation(string firstAnimName, string NextAnimName)
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
