using UnityEngine;

public class AmmoCache : MonoBehaviour, IInteractable
{
    public int AmmoSurplus;
    public WeaponStats weaponData;
    public AudioSource openingSound;

    public HighlightedObject highlights;
    bool canPickUp;

    void Start()
    {
        openingSound = GetComponent<AudioSource>();
        highlights = GetComponent<HighlightedObject>();
        canPickUp = true;
    }

    public void Update()
    {
        weaponData = FindAnyObjectByType<WeaponStats>();
    }

    public void Interact()
    {
        if (canPickUp)
        {
            weaponData.reserveAmmo = Mathf.Min(weaponData.reserveAmmo + AmmoSurplus, weaponData.maxAmmo * 3);
            openingSound.Play();
            canPickUp = false;
        }
    }
}
