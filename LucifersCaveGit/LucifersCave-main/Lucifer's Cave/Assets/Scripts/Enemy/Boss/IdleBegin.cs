using UnityEngine;

public class IdleBegin : MonoBehaviour
{
    private static readonly int IsIdleHash = Animator.StringToHash("isIdle");
    private Animator animator;
    [SerializeField] private float idleWaitTime = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RoarFinish()
    {
        animator.SetBool(IsIdleHash, true);

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
    }
}
