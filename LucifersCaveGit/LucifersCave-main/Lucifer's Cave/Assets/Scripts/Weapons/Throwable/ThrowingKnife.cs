using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    public float destructionDelay;

    public AudioSource throwingKnifeSound;
    public AudioSource impactSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        throwingKnifeSound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;
        else
            targetHit = true;

        rb.isKinematic = true;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        transform.SetParent(collision.transform);
        throwingKnifeSound.Stop();
        impactSound.Play();

        Destroy(gameObject, destructionDelay);
    }
}
