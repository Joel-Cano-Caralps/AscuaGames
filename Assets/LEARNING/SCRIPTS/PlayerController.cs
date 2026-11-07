using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region INPUT ACTIONS
    [SerializeField] InputActionAsset inputActions;

    InputAction moveAction;

    void OnEnable()  => moveAction.Enable();
    void OnDisable() => moveAction.Disable();
    #endregion

    [SerializeField] float maxSpeed = 8f;
    [SerializeField] float acceleration = 20f;

    Rigidbody rb;
    Transform cam;
    Vector3 lastForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = inputActions.FindAction("Move");
        cam = Camera.main.transform;
    }

    void FixedUpdate()
    {
        // WASD / flechas: X = A/D, Z = W/S
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude > 1f) input.Normalize();

        // Ejes de la cámara, planos (sin componente vertical)
        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Dirección relativa a la cámara
        Vector3 dir = camForward * input.y + camRight * input.x;
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 targetVel = dir * maxSpeed;

        // Acelera hacia la velocidad objetivo; ForceMode.Acceleration ignora la masa
        lastForce = (targetVel - currentVel) * acceleration;
        rb.AddForce(lastForce, ForceMode.Acceleration);
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + lastForce);
        Gizmos.DrawSphere(lastForce, 0.05f);
    }
}