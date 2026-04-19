using System.Collections;
using UnityEngine;

public class DoorLift : MonoBehaviour
{
    [Header("Time Settings")]
    public float descendHeight = 5f;
    public float descendDuration = 2f;

    [Header("Audio Settings")]
    public AudioSource doorDescendingAudio;

    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.85f;
    public float shakeAmount = 0.3f;

    [Header("Particle Effects")]
    public ParticleSystem dustParticles;

    public IEnumerator DescendDoor()
    {
        Debug.Log("Door descending initiated.");

        Vector3 targetPosition = transform.localPosition + Vector3.down * descendHeight;
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.localPosition;

        if (CameraShake.instance != null)
        {
            CameraShake.instance.StartCoroutine(CameraShake.instance.CameraShakeProcess(shakeDuration, shakeAmount));
        }

        if (dustParticles != null && !dustParticles.isPlaying)
        {
            dustParticles.Play();
        }

        while (elapsedTime < descendDuration)
        {
            transform.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / descendDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = targetPosition;
    }
}
