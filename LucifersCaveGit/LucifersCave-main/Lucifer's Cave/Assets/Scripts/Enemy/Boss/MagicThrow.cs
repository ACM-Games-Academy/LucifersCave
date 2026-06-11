using UnityEngine;

public class MagicThrow : MonoBehaviour
{
    private BossAttacks bossAttacks;
    public GameObject magicProjectilePrefab;
    public Transform magicSpawnPoint;
    public float throwForce = 10f;
    public float throwUpwardForceMagic = 5f;
    public float magicGrowthMultiplier = 3f;

    void Start()
    {
        bossAttacks = GetComponentInParent<BossAttacks>();
    }

    public void ThrowMagic()
    {
        GameObject magicProjectile = Instantiate(magicProjectilePrefab,
            magicSpawnPoint.position,
            magicSpawnPoint.rotation);

        magicProjectile.transform.parent = null;
        StartCoroutine(bossAttacks.IncreaseSize(magicProjectile, magicGrowthMultiplier));

        Rigidbody magicProjectileRB = magicProjectile.GetComponent<Rigidbody>();
        Vector3 direction = (bossAttacks.player.position - magicSpawnPoint.position).normalized;

        magicProjectileRB.AddForce(direction * throwForce + Vector3.up *
            throwUpwardForceMagic, ForceMode.Impulse);
    }
}
