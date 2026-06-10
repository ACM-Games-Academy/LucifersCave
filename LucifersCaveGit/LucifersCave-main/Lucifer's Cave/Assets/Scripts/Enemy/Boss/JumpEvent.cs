using UnityEngine;

public class JumpEvent : MonoBehaviour
{
    BossAudio bossAudio;
    public float shakeDuration = 0.8f;
    public float shakeAmount = 0.75f;

    public void JumpLand()
    {
        bossAudio = GetComponentInParent<BossAudio>();
        bossAudio.PlayEarthQuakeSound();
        CameraShake.instance.StartCoroutine(CameraShake.instance.CameraShakeProcess(shakeDuration, shakeAmount));
        BossAttacks bossAttacks = GetComponentInParent<BossAttacks>();
        if (bossAttacks != null)
        {
            float distanceToPlayer = Vector3.Distance(bossAttacks.player.position, transform.position);
            if (distanceToPlayer <= bossAttacks.jumpActivationDistance)
            {
                PlayerHealth playerHealth = bossAttacks.player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(bossAttacks.jumpDamage);
                }
            }
        }
    }
}
