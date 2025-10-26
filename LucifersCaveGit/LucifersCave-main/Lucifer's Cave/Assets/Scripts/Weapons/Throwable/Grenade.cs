using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 5f;
    public float blastRadius;
    public float explosionForce;
    public int damage;
    public ParticleSystem explosionEffect;
    public ParticleSystem dustExplosionEffect;

    private float countdown;
    private bool hasExploded = false;
    public AudioSource explosionSound;

    void Start()
    {
        countdown = delay;
        explosionSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    public void Explode()
    {
        if (explosionSound != null)
        {
            Debug.Log("Playing explosion sound");
            explosionSound.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found on Grenade!");
        }

        if (explosionEffect != null)
        {
            ParticleSystem ps = Instantiate(explosionEffect, transform.position, transform.rotation);
            ParticleSystem ds = Instantiate(dustExplosionEffect, transform.position, transform.rotation);
            ds.Play();
            ps.Play();

            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            Destroy(ds.gameObject, ds.main.duration + ds.main.startLifetime.constantMax);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearby in colliders)
        {
            EnemyHealth enemy = nearby.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Rigidbody body = nearby.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }

        Destroy(gameObject);
    }
}
