using System.Collections;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public float damageDelay;
    public float meleeDelay;
    public int attackDamage;
    public LayerMask zombieLyr;
    public GameObject attackPoint;
    public PlayerScore playerScore;

    [Header("Input")]
    public KeyCode attackKey;
    private bool isAttacking = false;

    [Header("Animation")]
    public Animator animator;

    [Header("Audio")]
    private AudioSource meleeSound;

    void Start()
    {
        playerScore = Object.FindAnyObjectByType<PlayerScore>();
        animator = GetComponent<Animator>();
        meleeSound = GetComponent<AudioSource>();
        attackPoint = transform.Find("attackPoint").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
            animator.SetTrigger("isAttackingMelee");
        }
    }

    public IEnumerator MeleeAttack()
    {
        if (attackPoint == null)
        {
            attackPoint = transform.Find("attackPoint").gameObject;
            yield break;
        }

        isAttacking = true;
        yield return new WaitForSeconds(damageDelay);
        meleeSound.Play();
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, zombieLyr);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(attackDamage);
                playerScore.AddPoints(playerScore.bodyShotPoints);
                FindFirstObjectByType<PointSpawner>().ShowPoints(playerScore.bodyShotPoints);

                if (enemyHealth.currentHealth <= 0)
                {
                    playerScore.AddPoints(enemyHealth.knifePoints);
                    FindFirstObjectByType<PointSpawner>().ShowPoints(enemyHealth.knifePoints);
                }
            }
        }

        yield return new WaitForSeconds(meleeDelay);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
