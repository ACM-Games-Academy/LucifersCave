using UnityEngine;
using TMPro;

public class InteractVial : MonoBehaviour, IInteractable
{
    private TextMeshProUGUI vialCounter;

    public void Interact()
    {
        if (int.TryParse(vialCounter.text, out int vialCount) && vialCount > 0)
        {
            vialCounter.text = (vialCount + 1).ToString();
        }
    }

    public void Initialize(TextMeshProUGUI vialCounter)
    {
        this.vialCounter = vialCounter;
    }
}
