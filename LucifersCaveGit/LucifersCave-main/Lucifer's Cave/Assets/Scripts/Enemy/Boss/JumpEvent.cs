using System.Collections;
using UnityEngine;

public class JumpEvent : MonoBehaviour
{
    BossAudio bossAudio;
    public float shakeDuration = 0.8f;
    public float shakeAmount = 0.75f;

    public float jumpAudioLength = 2f;
    public float damageRadius = 5f;

    public void JumpLand()
    {
        bossAudio = GetComponentInParent<BossAudio>();
        bossAudio.PlayEarthQuakeSound();
        CameraShake.instance.StartCoroutine(CameraShake.instance.CameraShakeProcess(shakeDuration, shakeAmount));
        BossAttacks bossAttacks = GetComponentInParent<BossAttacks>();
        if (bossAttacks != null)
        {
            float distanceToPlayer = Vector3.Distance(bossAttacks.player.position, transform.position);
            if (distanceToPlayer <= damageRadius)
            {
                PlayerHealth playerHealth = bossAttacks.player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(bossAttacks.jumpDamage);
                }
            }
        }

        StartCoroutine(FadeOutAudio(bossAudio.GetComponent<AudioSource>(), jumpAudioLength));
    }

    public IEnumerator FadeOutAudio(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
