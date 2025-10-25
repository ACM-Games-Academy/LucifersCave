using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class HighlightedObject : MonoBehaviour
{
    private MeshRenderer originalMesh;
    [SerializeField] private MeshRenderer highlightedMesh;

    void Start()
    {
        originalMesh = GetComponent<MeshRenderer>();
        highlightedMesh = GetComponentInChildren<MeshRenderer>();
        Highlight(false);
    }

    public void Highlight(bool state)
    {
        if (highlightedMesh != null)
        {
            highlightedMesh.enabled = state;
            originalMesh.enabled = !state;
        }
    }
}
