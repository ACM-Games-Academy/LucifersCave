using UnityEngine;

public class AmmoCache : MonoBehaviour, IInteractable
{
    private Initializer initializer;
    public int AmmoSurplus;
    public Reloading reloadingScript;
    public AudioSource openingSound;

    public HighlightedObject highlights;
    bool canPickUp;

    void Start()
    {
        openingSound = GetComponent<AudioSource>();
        highlights = GetComponent<HighlightedObject>();
        canPickUp = true;
    }

    public void Initialize(Reloading reloadingScript, Initializer initializer)   
    {
        this.reloadingScript = reloadingScript;
        this.initializer = initializer;
    }

    public void Interact()
    {
        if (!canPickUp) return;

        if (canPickUp)
        {
            if (reloadingScript == null) return;
            Debug.Log("Ammo Cache Picked Up");

            initializer.BindAmmoCache();
            reloadingScript.reserveAmmo = Mathf.Min(reloadingScript.reserveAmmo + AmmoSurplus, reloadingScript.maxAmmo * 3);
            reloadingScript.UpdateAmmo();
            openingSound.Play();
            canPickUp = false;
            Destroy(gameObject, openingSound.clip.length);
            var col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}