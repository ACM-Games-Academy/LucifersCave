using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public int attackDamage;
    public LayerMask zombieLyr;
    public Transform attackPoint;
    public plyrScore PlayerScore;

    public KeyCode attackKey;

    void Start()
    {
        PlayerScore = Object.FindAnyObjectByType<plyrScore>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            MeleeAttack();
        }
    }

    public void MeleeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, zombieLyr);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(attackDamage);
                PlayerScore.AddPoints(enemyHealth.knifePoints);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
