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
    public float stompDelay = 3.21f;
    public LayerMask playerLayer;

    [Header("Jump Attack")]
    public float jumpDamage = 30f;
    public float jumpCooldown = 3f;
    public float jumpActivationDistance = 6f;
    public float jumpDelay = 2.9f;

    [Header("Call Reinforcements")]
    public int reinforcementCount;
    public float reinforcementCooldown = 10f;
    public ParticleSystem instantiateCoverSmoke;
    public GameObject[] reinforcementPrefabs;
    public Transform[] spawnPoints;

    [Header("Magic Attacks")]
    public float magicAttackCooldown = 4f;
    public float magicAttackRange = 17f;
    public float magicThrowDelay = 5.19f;
    

    [Header("States, Animation and Audio")]
    private BossAudio bossAudio;
    private float nextAttackTime;
    private float idleEndTime;
    public float idleTime;
    private bool isAttacking;


    protected override void Start()
    {
        base.Start();

        RandomizeReinforcementCount();

        bossAudio = GetComponent<BossAudio>();
        idleEndTime = Time.time + idleTime;

        if (player == null)
            Debug.LogError("Player reference missing");

        if (animator == null)
            Debug.LogError("Animator missing");

        if (agent == null)
            Debug.LogError("NavMeshAgent missing");
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time >= idleEndTime && state == BossState.Idle)
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
        idleEndTime = Time.time + idleTime;
        state = BossState.Chasing;
    }

    public IEnumerator JumpAttack()
    {
        isAttacking = true;
        agent.isStopped = true;
        state = BossState.Jumping;

        animator.SetTrigger(JumpAttackHash);

        yield return new WaitForSeconds(jumpDelay);

        agent.isStopped = false;
        idleEndTime = Time.time + idleTime;
        state = BossState.Chasing;
        isAttacking = false;
    }

    public IEnumerator MagicAttack()
    {
        isAttacking = true;
        agent.isStopped = true;
        state = BossState.MagicAttacking;
        animator.SetTrigger(MagicHash);

        yield return new WaitForSeconds(magicThrowDelay);

        agent.isStopped = false;
        idleEndTime = Time.time + idleTime;
        state = BossState.Chasing;
        isAttacking = false;
    }

    public IEnumerator CallReinforcements()
    {
        isAttacking = true;
        state = BossState.Reinforcements;
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

        yield return new WaitForSeconds(2.9f);

        isAttacking = false;
        state = BossState.Chasing;
    }

    public void ExecuteAttack()
    {
        if (isAttacking) return;
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
        else if (distanceToPlayer <= jumpActivationDistance)
        {
            StartCoroutine(JumpAttack());
            nextAttackTime = Time.time + jumpCooldown;
        }
        else
        {
            StartCoroutine(CallReinforcements());
            nextAttackTime = Time.time + reinforcementCooldown;
        }

        Debug.Log(
        $"State={state}, Distance={distanceToPlayer}");
    }

    public void RandomizeReinforcementCount()
    {
        reinforcementCount = Random.Range(3, 6);
    }

    public IEnumerator IncreaseSize(GameObject targetToScale, float targetScaleMultiplier)
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
