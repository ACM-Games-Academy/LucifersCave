using UnityEngine;
using TMPro;

public class InteractVial : MonoBehaviour, IInteractable
{
    private TextMeshProUGUI vialCounter;

    public void Interact()
    {
        if (vialCounter == null)
        {
            Debug.LogError("Vial Counter reference is not set.");
            return;
        }

        if (int.TryParse(vialCounter.text, out int vialCount) && vialCount >= 5)
        {
            vialCounter.text = (vialCount + 1).ToString();
        }
    }

    public void Initialize(TextMeshProUGUI vialCounter)
    {
        this.vialCounter = vialCounter;
    }
}
