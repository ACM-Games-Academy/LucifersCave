using UnityEngine;

public class AmmoCache : MonoBehaviour, IInteractable
{
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

    public void Initialize(Reloading reloadingScript)   
    {
        this.reloadingScript = reloadingScript;
    }

    public void Interact()
    {
        if (!canPickUp) return;

        if (canPickUp)
        {
            if (reloadingScript == null) return;

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