using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("References")]
    public Movement moveScript;
    public Image staminaBar;

    [Header("Variables")]
    public float staminaDecreaseTime;
    public float depletedStaminaDelay;
    public float staminaDelay;
    public float staminaRegenerationSpeed;
    [SerializeField] private bool staminaDepleted;
    [SerializeField] private bool isRegenerating;

    private Coroutine regenerationCoroutine;

    void Start()
    {
        moveScript = GetComponent<Movement>();
    }

    void Update()
    {
        if (moveScript == null)
        {
            return;
        }

        bool isSprinting = Input.GetKey(moveScript.sprintKey) && moveScript.canSprint
            && moveScript.rb.linearVelocity.magnitude > 0.2f;

        if (isSprinting)
        {
            staminaBar.fillAmount -= staminaDecreaseTime * Time.deltaTime;
            staminaBar.fillAmount = Mathf.Clamp01(staminaBar.fillAmount);

            if (regenerationCoroutine != null)
            {
                StopCoroutine(regenerationCoroutine);
                regenerationCoroutine = null;
            }

            if (staminaBar.fillAmount <= 0)
            {
                moveScript.canSprint = false;
                staminaDepleted = true;
                regenerationCoroutine = StartCoroutine(StaminaRegeneration(depletedStaminaDelay));
            }
        }
        else
        {
            if (staminaBar.fillAmount < 1f && regenerationCoroutine == null)
            {
                regenerationCoroutine = StartCoroutine(StaminaRegeneration(staminaDelay));
            }
        }
    }

    IEnumerator StaminaRegeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        isRegenerating = true;

        while (staminaBar.fillAmount < 1f)
        {
            staminaBar.fillAmount += staminaRegenerationSpeed * Time.deltaTime;
            staminaBar.fillAmount = Mathf.Clamp01(staminaBar.fillAmount);
            yield return null;
        }

        isRegenerating = false;
        staminaDepleted = false;
        moveScript.canSprint = true;
        regenerationCoroutine = null;
    }
}
