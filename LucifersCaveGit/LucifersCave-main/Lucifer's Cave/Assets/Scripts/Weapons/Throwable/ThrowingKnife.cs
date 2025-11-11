using UnityEngine;

public class ThrowingKnife : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    public float destructionDelay;
    public int damage;

    public AudioSource throwingKnifeSound;
    public AudioSource impactSound;

    public PlayerScore playerScore;

    public void Initialize(PlayerScore playerScore)
    {
        this.playerScore = playerScore;
    }

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

        var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        enemyHealth.TakeDamage(damage);

        if (enemyHealth.currentHealth > 0)
        {
            playerScore.AddPoints(playerScore.bodyShotPoints);
            FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.bodyShotPoints);
        }
        else
        {
            playerScore.AddPoints(playerScore.deathPoints);
            FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.deathPoints);
        }

        transform.SetParent(collision.transform);
        throwingKnifeSound.Stop();
        impactSound.Play();

        Destroy(gameObject, destructionDelay);
    }
}
