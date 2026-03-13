using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    private PlayerScore playerScore;
    public LayerMask criticalLayer;

    public void Initialize(PlayerScore playerScore)
    {
        this.playerScore = playerScore;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable idamageable = collision.gameObject.GetComponent<IDamageable>();

        if (idamageable != null)
        {
            idamageable.TakeDamage(damage);
            playerScore.AddPoints(playerScore.bodyShotPoints);
            FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.bodyShotPoints);

            if ((criticalLayer.value & (1 << collision.gameObject.layer)) != 0)
            {
                int criticalDamage = damage * 2;
                idamageable.TakeDamage(criticalDamage);
            }

            if (idamageable.CurrentHealth <= 0)
            {
                playerScore.AddPoints(playerScore.deathPoints);
                FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.deathPoints);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Grenade grenadeScript)) 
        {
            grenadeScript.Explode();
        }

        Destroy(gameObject);
    }
}