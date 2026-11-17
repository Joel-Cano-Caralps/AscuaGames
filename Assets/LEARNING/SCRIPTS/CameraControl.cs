using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.15f;
    [SerializeField] bool invertX = false;
    [SerializeField] bool invertY = false;
    [SerializeField] float minPitch = -30f;
    [SerializeField] float maxPitch = 60f;
    float yaw;
    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        if (pitch > 180f) pitch -= 360f;
    }

    void LateUpdate()
    {
        Mouse mouse = Mouse.current;
        if (mouse != null)
        {
            Vector2 delta = mouse.delta.ReadValue();
            yaw += delta.x * sensitivity * (invertX ? -1f : 1f);
            pitch -= delta.y * sensitivity * (invertY ? -1f : 1f);
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}