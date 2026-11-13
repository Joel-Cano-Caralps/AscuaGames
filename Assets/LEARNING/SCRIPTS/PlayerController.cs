using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
    #region ENUMS
    private enum PlayerState { Idle, Walking, Running }
    #endregion

    #region INPUT
    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;

    InputAction moveAction;
    InputAction sprintAction;
    #endregion

    #region SETTINGS
    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float acceleration = 20f;

    [HorizontalLine]

    [SerializeField] float slopeAdjustSpeed = 60f;
    [SerializeField] float rotationSpeed = 360f;
    #endregion

    #region REFERENCES
    Rigidbody rb;
    Transform cam;
    Animator animator;
    #endregion
    PlayerState currentState = PlayerState.Idle;
    Vector3 force;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();

        moveAction = inputActions.FindAction("Move");
        sprintAction = inputActions.FindAction("Sprint");
    }

    void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
    }

    void FixedUpdate()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input.sqrMagnitude > 1f) input.Normalize();


        Rotation();
        Movement(input);
        UpdateState(input);
    }

    void Update()
    {
        SetAnimations();
    }

    #region ROTATION
    void Rotation()
    {
        Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.green);
        Vector3 reflectedVec = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f))
        {
            reflectedVec = hit.normal;
        }
        Debug.Log(Vector3.Angle(Vector3.up, reflectedVec));
        Vector3 horizontalDir = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalDir.sqrMagnitude < 0.01f) return;

        Vector3 slopeDir = Vector3.ProjectOnPlane(horizontalDir, reflectedVec);
        if (slopeDir.sqrMagnitude < 0.01f) return;

        Quaternion horizontalRot = Quaternion.LookRotation(horizontalDir, reflectedVec);
        Quaternion afterYaw = Quaternion.RotateTowards(rb.rotation, horizontalRot, rotationSpeed * Time.fixedDeltaTime);

        Quaternion slopeRot = Quaternion.LookRotation(slopeDir, reflectedVec);
        rb.MoveRotation(Quaternion.RotateTowards(afterYaw, slopeRot, slopeAdjustSpeed * Time.fixedDeltaTime));
    }

    #endregion
    #region MOVEMENT
    void Movement(Vector2 input)
    {
        //GET CAMERA DIRECTION
        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 dir = camForward * input.y + camRight * input.x;
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        //ADD FORCES
        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 targetVel = dir * (sprintAction.IsPressed() ? runSpeed : walkSpeed);

        Debug.Log(rb.linearVelocity);

        force = (targetVel - currentVel) * acceleration;
        rb.AddForce(force, ForceMode.Acceleration);
    }

    void UpdateState(Vector2 input)
    {
        bool isMoving = input.sqrMagnitude > 0.01f && rb.linearVelocity.sqrMagnitude > 0.01f;

        if (!isMoving)
        {
            currentState = PlayerState.Idle;
            return;
        }

        currentState = sprintAction.IsPressed() ? PlayerState.Running : PlayerState.Walking;
    }
    #endregion

    #region ANIMATION
    void SetAnimations()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("isMoving", false);
                animator.SetBool("isRunning", false);
                break;

            case PlayerState.Walking:
                animator.SetBool("isMoving", true);
                animator.SetBool("isRunning", false);
                break;

            case PlayerState.Running:
                animator.SetBool("isMoving", true);
                animator.SetBool("isRunning", true);
                break;
        }
    }
    #endregion

    #region GROUND CHECK
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f);
    }
    #endregion

    #region DEBUG
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force);
        Gizmos.DrawSphere(transform.position + force, 0.05f);
    }
    #endregion
}