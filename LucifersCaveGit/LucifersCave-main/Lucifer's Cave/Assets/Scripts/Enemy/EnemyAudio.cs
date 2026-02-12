using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip[] attackClips; 
    public AudioClip[] deathClips; 
    public AudioClip[] hurtClips;
    public AudioClip[] idleClips;

    private AudioSource audioSource;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth;

    private bool wasAttacking;
    private bool wasDead;
    private bool wasAttacked;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>(); 
    }

    void Update()
    {
        if (enemyAttack != null && enemyAttack.isAttacking && !wasAttacking)
        {
            PlayAttackSound();
            wasAttacking = true;
        }
        else if (enemyHealth != null && enemyHealth.isDead && !wasDead) 
        {
            PlayDeathSound();
            wasDead = true;
        }
        else if (enemyHealth != null && enemyHealth.isHurt && !wasAttacked) 
        {   
            PlayHurtSound();
            wasAttacked = true;
        }

        if (enemyAttack != null && !enemyAttack.isAttacking) { wasAttacking = false; }
        if (enemyHealth != null && !enemyHealth.isHurt) { wasAttacked = false; }
    }

    void PlayAttackSound() => PlayRandomClip(attackClips);
    void PlayDeathSound() => PlayRandomClip(deathClips);
    void PlayHurtSound() => PlayRandomClip(hurtClips);
    void PlayIdleSound() => PlayRandomClip(idleClips);

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
    }
}
