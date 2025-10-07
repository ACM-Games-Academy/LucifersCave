using UnityEngine;

public class Bullets : MonoBehaviour
{
    public WeaponStats weaponStats;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth zombie = collision.gameObject.GetComponent<EnemyHealth>();

        if (zombie != null)
        {
            zombie.TakeDamage((int)weaponStats.damage);
        }

        Destroy(gameObject);
    }
}

