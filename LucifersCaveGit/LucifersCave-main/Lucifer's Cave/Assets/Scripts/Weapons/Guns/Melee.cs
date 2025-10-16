using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public float attackDelay;
    public int attackDamage;
    public LayerMask zombieLyr;
    public Transform attackPoint;
    public PlayerScore playerScore;

    [Header("Input")]
    public KeyCode attackKey;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        playerScore = Object.FindAnyObjectByType<PlayerScore>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            MeleeAttack();
            animator.SetTrigger("isAttackingMelee");
        }
    }

    public IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, zombieLyr);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(attackDamage);
                playerScore.AddPoints(enemyHealth.knifePoints);
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
