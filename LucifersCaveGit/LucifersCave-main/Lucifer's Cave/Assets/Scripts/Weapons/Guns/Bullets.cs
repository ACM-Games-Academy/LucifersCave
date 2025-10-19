using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damage;
    public PlayerScore playerScore;

    public void Initialize(PlayerScore playerScore)
    {
        this.playerScore = playerScore;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth zombie = collision.gameObject.GetComponent<EnemyHealth>();

        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            playerScore.AddPoints(playerScore.bodyShotPoints);
        }

        Destroy(gameObject);
    }
}

