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

    public AudioSource audioSource;
    public AudioSource musicSource;

    private void Start()
    {
        musicSource.clip = bossMusic; musicSource.Play();
    }

    public void PlayRoarSound()
    {
        audioSource.PlayOneShot(roarSound);
    }

    public void PlayStompSound()
    {
        audioSource.PlayOneShot(stompSound);
    }

    public void PlayEarthQuakeSound()
    {
        audioSource.PlayOneShot(earthQuakeSound);
    }

    public void PlayLaserSound()
    {
        audioSource.PlayOneShot(laserSound);
    }

    public void PlayMinionSpawningSound()
    {
        audioSource.PlayOneShot(minionSpawningSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
}
