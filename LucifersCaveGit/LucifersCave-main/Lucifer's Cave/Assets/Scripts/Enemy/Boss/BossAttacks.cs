using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : BossBase, IMagicAttack, IStompAttack, IJumpAttack, ICallReinforcements
{
    private static readonly int RoarHash = Animator.StringToHash("Roar");
    private static readonly int MagicHash = Animator.StringToHash("Magic");
    private static readonly int JumpAttackHash = Animator.StringToHash("jumpAttack");
    private static readonly int StompAttackHash = Animator.StringToHash("stompAttack");

    [Header("Stomp Attack")]
    public float stompDamage = 20f;
    public float stompCooldown = 5f;
    public float stompActivationDistance = 3f;
    public float stompDelay = 0.8f;
    public LayerMask playerLayer;

    [Header("Jump Attack")]
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
    public float magicGrowthMultiplier = 3f;
    public float magicAttackRange = 10f;
    public float magicThrowDelay = 1.2f;
    public float throwForce = 10f;
    public float throwUpwardForceMagic = 5f;

    [Header("States, Animation and Audio")]
    private BossAudio bossAudio;
    private float nextAttackTime;
    public float idleTime;


    protected override void Start()
    {
        base.Start();

        RandomizeReinforcementCount();
        rb = GetComponent<Rigidbody>();

        bossAudio = GetComponent<BossAudio>();

        if (player == null)
            Debug.LogError("Player reference missing");

        if (animator == null)
            Debug.LogError("Animator missing");

        if (agent == null)
            Debug.LogError("NavMeshAgent missing");

        if (rb == null)
            Debug.LogError("Rigidbody missing");
    }

    protected override void Update()
    {
        base.Update();

        float currentTime = Time.time;
        if (currentTime >= idleTime && state == BossState.Idle)
        {
            state = BossState.Chasing;
        }

        if (state == BossState.Chasing)
        {
            ExecuteAttack();
        }
    }

    public IEnumerator StompAttack()
    {
        agent.isStopped = true;
        state = BossState.Stomping;
        animator.SetTrigger(StompAttackHash);

        yield return new WaitForSeconds(stompDelay);
        agent.isStopped = false;
        state = BossState.Idle;
    }

    public IEnumerator JumpAttack()
    {
        agent.isStopped = true;
        state = BossState.Jumping;

        animator.SetTrigger(JumpAttackHash);

        yield return null;
        agent.isStopped = false;
        state = BossState.Idle;
    }

    public IEnumerator MagicAttack()
    {
        state = BossState.MagicAttacking;
        animator.SetTrigger(MagicHash);
        GameObject magicProjectile = Instantiate(magicProjectilePrefab, 
            magicSpawnPoint.position, 
            magicSpawnPoint.rotation);

        StartCoroutine(IncreaseSize(magicProjectile, magicGrowthMultiplier));

        yield return new WaitForSeconds(magicThrowDelay);

        magicProjectile.transform.parent = null;
        Rigidbody magicProjectileRB = magicProjectile.GetComponent<Rigidbody>();
        Vector3 direction = (player.position - magicSpawnPoint.position).normalized;

        magicProjectileRB.AddForce(direction * throwForce + Vector3.up * 
            throwUpwardForceMagic, ForceMode.Impulse);

        state = BossState.Idle;
    }

    public void CallReinforcements()
    {
        RandomizeReinforcementCount();
        animator.SetTrigger(RoarHash);
        bossAudio.PlayEarthQuakeSound();
        bossAudio.PlayMinionSpawningSound();

        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < reinforcementCount && availableSpawnPoints.Count > 0; i++)
        {
            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);

            Transform spawn = availableSpawnPoints[spawnIndex];

            availableSpawnPoints.RemoveAt(spawnIndex);

            Instantiate(instantiateCoverSmoke, spawn.position, Quaternion.identity);
            int randomEnemy = Random.Range(0, reinforcementPrefabs.Length);
            Instantiate(reinforcementPrefabs[randomEnemy], spawn.position, Quaternion.identity);
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
        else if (distanceToPlayer <= magicAttackRange)
        {
            StartCoroutine(MagicAttack());
            nextAttackTime = Time.time + magicAttackCooldown;
        }
        else
        {
            StartCoroutine(JumpAttack());
            nextAttackTime = Time.time + jumpCooldown;
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

    private IEnumerator IncreaseSize(GameObject targetToScale, float targetScaleMultiplier)
    {
        Vector3 originalScale = targetToScale.transform.localScale;
        Vector3 targetScale = originalScale * targetScaleMultiplier;
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            targetToScale.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetToScale.transform.localScale = targetScale;
    }
}
