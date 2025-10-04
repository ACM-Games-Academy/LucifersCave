using UnityEngine;

public class PickUpDrop : MonoBehaviour
{
    public Melee meleeScript;
    public Transform itemHandler;
    public Transform player;
    public Transform plyrCam;

    public Rigidbody rb;
    public BoxCollider boxColl;

    public float dropForwardForce;
    public float dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Start()
    {
        if (!equipped)
        {
            meleeScript.enabled = false;
            rb.isKinematic = false;
            boxColl.isTrigger = false;
        }
        if (equipped)
        {
            meleeScript.enabled = true;
            rb.isKinematic = true;
            boxColl.isTrigger = true;
        }
    }


    void Update()
    {
        if (!equipped && !slotFull)
        {

        }
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        boxColl.isTrigger = false;

        rb.linearVelocity = player.GetComponent<Rigidbody>().linearVelocity;
        rb.AddForce(plyrCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(plyrCam.up * dropUpwardForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        meleeScript.enabled = false;
    }

    public void Interact()
    {
        equipped = true;
        slotFull = true;

        transform.SetParent(itemHandler);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        boxColl.isTrigger = true;

        meleeScript.enabled = true;
    }
}
