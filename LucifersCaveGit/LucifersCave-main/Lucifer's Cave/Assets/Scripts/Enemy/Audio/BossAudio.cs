using UnityEngine;

public class BossAudio : MonoBehaviour
{
    public AudioClip bossMusic;
    public AudioClip stompSound;
    public AudioClip roarSound;
    public AudioClip laserSound;
    public AudioClip earthQuakeSound;
    public AudioClip minionSpawningSound;
    public AudioClip deathSound;

    public void PlayRoarSound()
    {
        AudioSource.PlayClipAtPoint(roarSound, transform.position);
    }

    public void PlayStompSound()
    {
        AudioSource.PlayClipAtPoint(stompSound, transform.position);
    }

    public void PlayEarthQuakeSound()
    {
        AudioSource.PlayClipAtPoint(earthQuakeSound, transform.position);
    }

    public void PlayLaserSound()
    {
        AudioSource.PlayClipAtPoint(laserSound, transform.position);
    }

    public void PlayMinionSpawningSound()
    {
        AudioSource.PlayClipAtPoint(minionSpawningSound, transform.position);
    }

    public void PlayDeathSound()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }
}
