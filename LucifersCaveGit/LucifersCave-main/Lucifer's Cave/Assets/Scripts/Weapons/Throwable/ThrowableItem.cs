using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThrowableItem : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject Grenade;
    public GameObject throwingKnife;

    [Header("Settings")]
    public int totalGrenadeThrows;
    public int totalKnifeThrows;
    public float throwCoolDown;

    [Header("Throwing")]
    public KeyCode grenadeThrowKey = KeyCode.G;
    public KeyCode knifeThrowKey = KeyCode.E;
    public float throwForce;
    public float throwUpwardForceGrenade;
    public float throwUpwardForceKnife;
    bool readyToThrow;

    [Header("Throwable UI")]
    public TextMeshProUGUI knivesCounter;
    public TextMeshProUGUI grenadeCounter;

    void Start()
    {
        readyToThrow = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(grenadeThrowKey) && readyToThrow && totalGrenadeThrows > 0)
        {
            ThrowGrenade();
        }

        if (Input.GetKeyDown(knifeThrowKey) && readyToThrow && totalKnifeThrows > 0)
        {
            ThrowKnife();
        }
    }

    private void ThrowGrenade()
    {
        readyToThrow = false;

        GameObject grenade = Instantiate(Grenade, attackPoint.position, cam.rotation);

        Rigidbody grenadeRB = grenade.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForceGrenade;
        grenadeRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalGrenadeThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
        GrenadeHUD_Update();
    }

    public void ThrowKnife()
    {
        readyToThrow = false;

        GameObject knife = Instantiate(throwingKnife, attackPoint.position, cam.rotation);

        Rigidbody knifeRB = knife.GetComponent<Rigidbody>();

        Vector3 forceToAdd = cam.transform.forward * throwForce + transform.up * throwUpwardForceKnife;
        knifeRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnifeThrows--;

        Invoke(nameof(ResetThrow), throwCoolDown);
        KnivesHUD_Update();
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    public void GrenadeHUD_Update()
    {
        if (grenadeCounter != null)
        {
            grenadeCounter.text = (totalGrenadeThrows.ToString());
        }
    }

    public void KnivesHUD_Update()
    {
        if (knivesCounter != null)
        {
            knivesCounter.text = (totalKnifeThrows.ToString());
        }
    }
}