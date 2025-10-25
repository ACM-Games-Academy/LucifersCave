using TMPro;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interact : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    private HighlightedObject lastHighlighted;

    public TextMeshProUGUI interactPrompt;
    public KeyCode interactKey = KeyCode.F;

    private void Start()
    {
        interactPrompt.enabled = false;
    }

    void Update()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.Highlight(false);
            lastHighlighted = null;
        }

        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            interactPrompt.enabled = true;

            if (Input.GetKeyDown(interactKey))
            {
                Debug.Log("Interact Input");

                if (hitInfo.collider.TryGetComponent(out HighlightedObject highlight))
                {
                    highlight.Highlight(true);
                    lastHighlighted = highlight;
                }

                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    Debug.Log("Interacted on Object");
                }
            }
        }
    }
}
