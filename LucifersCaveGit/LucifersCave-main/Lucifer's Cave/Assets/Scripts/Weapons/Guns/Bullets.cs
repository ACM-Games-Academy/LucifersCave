using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int Damage;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth zombie = collision.gameObject.GetComponent<EnemyHealth>();

        if (zombie != null)
        {
            zombie.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}

