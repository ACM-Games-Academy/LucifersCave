using System.Collections;
using UnityEngine;
using System.Linq;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public float damageDelay;
    public float meleeDelay;
    public int attackDamage;
    public LayerMask zombieLyr;
    public PlayerScore playerScore;
    [SerializeField] private Transform attackPoint;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !isAttacking)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    public IEnumerator MeleeAttack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("AttackPoint not assigned!", this);
            yield break;
        }

        animator.SetBool("Meleeing", true);
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
        animator.SetBool("Meleeing", false);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Transform attackPoint = transform.Find("attackPoint");

        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    public void SetKnife(Transform knifeRoot)
    {
        attackPoint = knifeRoot.GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t.name == "attackPoint");

        if (attackPoint == null)
            Debug.LogError("attackPoint not found in knife prefab!", knifeRoot);
    }
}
