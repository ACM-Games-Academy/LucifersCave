using UnityEngine;

public class IdleBegin : MonoBehaviour
{
    private static readonly int IsIdleHash = Animator.StringToHash("isIdle");
    [SerializeField] private float idleWaitTime = 2f;

    public static bool isIdle = true;
    BossAudio bossAudio;
    BossAttacks attacks;

    private void Start()
    {
        attacks = GetComponentInParent<BossAttacks>();
        attacks.animator = GetComponent<Animator>();
        bossAudio = GetComponentInParent<BossAudio>();
        bossAudio.PlayRoarSound();
    }

    public void RoarFinish()
    {
        attacks.animator.SetBool(IsIdleHash, true);
        attacks.state = BossBase.BossState.Idle;

        float elapsedTime = 0f;
        while (elapsedTime < idleWaitTime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= idleWaitTime)
            {
                attacks.animator.SetBool(IsIdleHash, false);
            }
        }

        elapsedTime = 0f;
        isIdle = false;
        attacks.state = BossBase.BossState.Chasing;
    }
}
