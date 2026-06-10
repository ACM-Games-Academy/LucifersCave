using UnityEngine;

public class StompEvent : MonoBehaviour
{
    public float stompRange = 3f;
    public int stompDamage = 20;
    public LayerMask playerLayer;

    BossAudio bossAudio;

    public void StompLand()
    {
        bossAudio = GetComponentInParent<BossAudio>();

        Collider playerHit = Physics.OverlapSphere(transform.position, stompRange, playerLayer)[0];
        playerHit.GetComponent<PlayerHealth>().TakeDamage(stompDamage);
        bossAudio.PlayStompSound();
    }
}
