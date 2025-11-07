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

    public PlayerScore playerScore;

    public void Initialize(PlayerScore playerScore)
    {
        this.playerScore = playerScore;
    }

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
        if (explosionSound != null && explosionSound.clip != null)
        {
            Debug.Log("Playing explosion sound from grenade prefab");

            GameObject soundObj = new GameObject("ExplosionSound");
            soundObj.transform.position = transform.position;

            AudioSource tempSource = soundObj.AddComponent<AudioSource>();
            tempSource.clip = explosionSound.clip;
            tempSource.volume = explosionSound.volume;
            tempSource.pitch = explosionSound.pitch;
            tempSource.spatialBlend = explosionSound.spatialBlend;
            tempSource.minDistance = explosionSound.minDistance;
            tempSource.maxDistance = explosionSound.maxDistance;
            tempSource.rolloffMode = explosionSound.rolloffMode;

            tempSource.Play();
            Destroy(soundObj, tempSource.clip.length);
        }
        else
        {
            Debug.LogWarning("No AudioSource or AudioClip found on Grenade prefab!");
        }

        if (explosionEffect != null)
        {
            ParticleSystem ps = Instantiate(explosionEffect, transform.position, transform.rotation);
            ParticleSystem ds = Instantiate(dustExplosionEffect, transform.position, transform.rotation);
            ps.Play();
            ds.Play();

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

                if (enemy.currentHealth > 0)
                {
                    playerScore.AddPoints(playerScore.bodyShotPoints);
                    FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.bodyShotPoints);
                }
                else
                {
                    playerScore.AddPoints(playerScore.deathPoints);
                    FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.deathPoints);
                }
            }

            Rigidbody body = nearby.GetComponent<Rigidbody>();
            if (body != null)
                body.AddExplosionForce(explosionForce, transform.position, blastRadius);
        }

        Destroy(gameObject);
    }
}
