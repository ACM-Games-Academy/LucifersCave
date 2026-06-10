using UnityEngine;

public class IdleBegin : BossBase
{
    private static readonly int IsIdleHash = Animator.StringToHash("isIdle");
    [SerializeField] private float idleWaitTime = 2f;

    public static bool isIdle = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RoarFinish()
    {
        animator.SetBool(IsIdleHash, true);
        state = BossState.Idle;

        float elapsedTime = 0f;
        while (elapsedTime < idleWaitTime)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= idleWaitTime)
            {
                animator.SetBool(IsIdleHash, false);
            }
        }

        elapsedTime = 0f;
        isIdle = false;
    }
}
