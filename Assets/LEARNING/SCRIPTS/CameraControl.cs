using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.15f;
    [SerializeField] bool invertX = false;

    float yaw;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        Mouse mouse = Mouse.current;
        if (mouse != null)
        {
            float dx = mouse.delta.ReadValue().x;
            yaw += dx * sensitivity * (invertX ? -1f : 1f);
        }

        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }
}