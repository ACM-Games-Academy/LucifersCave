using UnityEngine;

public class AmmoCache : MonoBehaviour, IInteractable
{
    private Initializer initializer;
    public int AmmoSurplus;
    public AudioSource openingSound;
    private WeaponManager weaponManager;

    public HighlightedObject highlights;
    bool canPickUp;

    void Start()
    {
        openingSound = GetComponent<AudioSource>();
        highlights = GetComponent<HighlightedObject>();
        initializer = FindFirstObjectByType<Initializer>();
        weaponManager = initializer.weaponManager;
        canPickUp = true;
    }

    public void Interact()
    {
        if (!canPickUp) return;
        if (initializer == null || initializer.weaponManager == null) return;

        Reloading reloadingScript = initializer.weaponManager.currentReloading;

        if (reloadingScript == null) return;

        reloadingScript.reserveAmmo = Mathf.Min(reloadingScript.reserveAmmo + AmmoSurplus,
            reloadingScript.maxAmmo * 3);

        weaponManager.gunBase.AddAmmo(AmmoSurplus);
        reloadingScript.UpdateAmmo();
        
        openingSound.Play();
        canPickUp = false;
        var col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        openingSound.transform.parent = null;
        Destroy(gameObject, openingSound.clip.length);
    }
}