using UnityEngine;

public class BossAttacks : BossBase
{
    [Header("Stomp Attack")]
    public float stompRange = 5f;
    public float stompDamage = 20f;
    public float stompCooldown = 5f;
    public float stompActivationDistance = 3f;
    public LayerMask playerLayer;

    [Header("Jump Attack")]
    public float jumpHeight = 5f;
    public float jumpDamage = 30f;
    public float jumpCooldown = 3f;
    public float jumpActivationDistance = 15f;

    [Header("Call Reinforcements")]
    public int reinforcementCount;
    public ParticleSystem instantiateCoverSmoke;
    public GameObject[] reinforcementPrefabs;
    public Transform[] spawnPoints;

    [Header("Magic Attacks")]
    public GameObject magicProjectilePrefab;
    public float magicAttackCooldown = 4f;
    public float magicAttackRange = 10f;
    public float magicAttackDamage = 45f;

    void Start()
    {
        reinforcementCount = Random.Range(3, 6);
    }

    void Update()
    {
        
    }
}
