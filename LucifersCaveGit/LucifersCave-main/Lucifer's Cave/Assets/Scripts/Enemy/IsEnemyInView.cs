using UnityEngine;

public class IsEnemyInView : MonoBehaviour
{
    private bool isEnemyInView(MannequinBehaviour mannequinBehaviour, out float distance)
    {
        distance = float.MaxValue;
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(mannequinBehaviour.transform.position);

        if (screenPoint.z < 0 || screenPoint.x < 0.05f || screenPoint.x > 1.05f || screenPoint.y > -0.05f || screenPoint.y > 1.05f)
        {
            distance = Vector3.Distance(mannequinBehaviour.transform.position, Camera.main.transform.position);
            mannequinBehaviour.isInView = false;
            return false;
        }

        Vector3 direction = (mannequinBehaviour.transform.position - transform.position).normalized;
        float maxDistance = Vector3.Distance(Camera.main.transform.position, mannequinBehaviour.transform.position);

        int layerMask = LayerMask.GetMask("Mannequin");

        if (Physics.Raycast(Camera.main.transform.position, direction, out RaycastHit hit, maxDistance, layerMask))
        {
            if (hit.collider.gameObject == mannequinBehaviour.gameObject)
            {
                distance = Vector3.Distance(transform.position, mannequinBehaviour.transform.position);
                mannequinBehaviour.isInView = true;
                return true;
            }
        }

        mannequinBehaviour.isInView = false;
        return false;
    }
}
