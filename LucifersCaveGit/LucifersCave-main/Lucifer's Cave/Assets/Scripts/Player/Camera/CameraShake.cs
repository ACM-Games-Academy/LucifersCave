using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.position;
    }

    public IEnumerator CameraShakeProcess(float shakeDuration, float shakeAmount)
    {
        float runningTime = 0f;

        while (runningTime < shakeDuration)
        {
            runningTime += Time.deltaTime;
            transform.position = initialPos + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }

        yield break;
    }
}
