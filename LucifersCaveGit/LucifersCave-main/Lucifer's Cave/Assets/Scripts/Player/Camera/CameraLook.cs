using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float sensitivityAmount = 100f;
    [Range(10, 50)] public float smoothAmount = 27;

    private float xRotation;
    private float yRotation;

    private Vector2 currentRotation;

    public Transform orientation;

    [SerializeField] private WeaponRecoil weaponRecoil;

    public static bool canLook = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (canLook)
        {
            // Raw input
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityAmount * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityAmount * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            currentRotation.x = Mathf.Lerp(currentRotation.x, xRotation, smoothAmount * Time.deltaTime);
            currentRotation.y = Mathf.Lerp(currentRotation.y, yRotation, smoothAmount * Time.deltaTime);

            Vector3 recoilRotation = weaponRecoil != null ? weaponRecoil.GetCurrentGunRotation() : Vector3.zero;

            transform.rotation = Quaternion.Euler(
                currentRotation.x + recoilRotation.x,
                currentRotation.y + recoilRotation.y,
                0);

            orientation.rotation = Quaternion.Euler(0, currentRotation.y, 0);
        }
    }
}