using System.Collections;
using UnityEngine;

public class BossAttacks : BossBase, IMagicAttack, IStompAttack, IJumpAttack, ICallReinforcements
{
    [Header("Stomp Attack")]
    public float stompRange = 5f;
    public float stompDamage = 20f;
    public float stompCooldown = 5f;
    public float stompActivationDistance = 3f;
    public float stompDelay = 0.8f;
    public LayerMask playerLayer;

    [Header("Jump Attack")]
    public float jumpHeight = 5f;
    public float jumpForce = 7f;
    public float jumpDamage = 30f;
    public float jumpCooldown = 3f;
    public float jumpActivationDistance = 15f;
    private Rigidbody rb;

    [Header("Call Reinforcements")]
    public int reinforcementCount;
    public ParticleSystem instantiateCoverSmoke;
    public GameObject[] reinforcementPrefabs;
    public Transform[] spawnPoints;

    [Header("Magic Attacks")]
    public GameObject magicProjectilePrefab;
    public Transform magicSpawnPoint;
    public float magicAttackCooldown = 4f;
    public float magicAttackRange = 10f;
    public float magicThrowDelay = 1.2f;
    public float throwForce = 10f;
    public float throwUpwardForceMagic = 5f;

    [Header("States and Animation Settings")]
    private float nextAttackTime;
    private bool isAttacking;

    void Start()
    {
        RandomizeReinforcementCount();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isAttacking)
            return;
    }

    public IEnumerator StompAttack()
    {
        agent.isStopped = true;
        isAttacking = true;
        animator.SetTrigger("stompAttack");

        yield return new WaitForSeconds(stompDelay);

        Collider playerHit = Physics.OverlapSphere(transform.position, stompRange, playerLayer)[0];

        isAttacking = false;
    }

    public IEnumerator JumpAttack(Transform playersLastPosition)
    {
        agent.isStopped = true;
        isAttacking = true;
        

        animator.SetTrigger("jumpAttack");
        isAttacking = false;
    }

    public Transform GetPlayersLastPosition()
    {
        return player;
    }

    public IEnumerator MagicAttack(Transform playersLastPosition)
    {
        isAttacking = true;
        animator.SetTrigger("Magic");
        GameObject magicProjectile = Instantiate(magicProjectilePrefab, 
            magicSpawnPoint.position, 
            magicSpawnPoint.rotation);

        yield return new WaitForSeconds(magicThrowDelay);

        magicProjectile.transform.parent = null;
        Rigidbody magicProjectileRB = magicProjectile.GetComponent<Rigidbody>();
        Vector3 direction = (playersLastPosition.position - magicSpawnPoint.position).normalized;

        magicProjectileRB.AddForce(direction * throwForce + Vector3.up * 
            throwUpwardForceMagic, ForceMode.Impulse);

        isAttacking = false;
    }

    public void CallReinforcements()
    {
        RandomizeReinforcementCount();
        animator.SetTrigger("Roar");

        foreach (Transform SpawnPoint in spawnPoints)
        {
            Instantiate(instantiateCoverSmoke, SpawnPoint.position, Quaternion.identity);
            int randomEnemy = Random.Range(0, reinforcementPrefabs.Length);
            Instantiate(reinforcementPrefabs[randomEnemy], SpawnPoint.position, Quaternion.identity);
        }
    }

    public void ExecuteAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (distanceToPlayer <= stompActivationDistance)
        {
            StartCoroutine(StompAttack());
            nextAttackTime = Time.time + stompCooldown;
        }
        else if (distanceToPlayer <= jumpActivationDistance)
        {
            StartCoroutine(JumpAttack(GetPlayersLastPosition()));
            nextAttackTime = Time.time + jumpCooldown;
        }
        else
        {
            StartCoroutine(MagicAttack(GetPlayersLastPosition()));
            nextAttackTime = Time.time + magicAttackCooldown;
        }
    }

    public void RandomizeReinforcementCount()
    {
        reinforcementCount = Random.Range(3, 6);
    }

    private void OnDrawGizmosSelected()
    {
        Transform attackPoint;
        if (state == BossState.Stomping)
        {
            attackPoint = transform.Find("stompAttackPoint");
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, stompActivationDistance);
        }
        else if (state == BossState.Jumping)
        {
            attackPoint = transform.Find("jumpAttackPoint");
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, jumpActivationDistance);
        }
    }
}
