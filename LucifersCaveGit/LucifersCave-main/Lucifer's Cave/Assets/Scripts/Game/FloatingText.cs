using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 30f;
    public float lifetime = 1f;
    public float fadeDuration = 0.5f;
    public float xValue;

    private TextMeshProUGUI textMesh;
    private Vector3 floatDirection;
    private float timer;

    public void Init(string text, Vector3 spawnPosition, Vector3 counterPosition)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = text;

        transform.position = spawnPosition;

        floatDirection = new Vector3(0.3f, 1f, 0f).normalized;

        floatDirection += new Vector3(xValue, Random.Range(-12, 12), 0);
        floatDirection.Normalize();
    }

    void Update()
    {
        transform.Translate(floatDirection * moveSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            float fade = 1 - ((timer - lifetime) / fadeDuration);
            Color c = textMesh.color;
            c.a = fade;
            textMesh.color = c;

            if (fade <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
