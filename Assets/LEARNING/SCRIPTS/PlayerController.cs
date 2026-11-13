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

    [SerializeField] float slopeAdjustSpeed = 2f;
    [SerializeField] float rotationSpeed = 360f;
    #endregion

    #region REFERENCES
    Rigidbody rb;
    Transform cam;
    Animator animator;
    #endregion
    PlayerState currentState = PlayerState.Idle;
    Vector3 force;
    Vector3 groundNormal = Vector3.up;

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

        groundNormal = GetSlopeNormal();

        Rotation();
        Movement(input);
        UpdateState(input);
    }

    void Update()
    {
        SetAnimations();
    }

    Vector3 GetSlopeNormal()
    {
        Vector3 slopeNormal = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f))
        {
            slopeNormal = hit.normal;
        }
        return slopeNormal;
    }

    #region ROTATION
        void Rotation()
        {
            Vector3 forward = Vector3.ProjectOnPlane(rb.rotation * Vector3.forward, groundNormal);
            Quaternion tiltRot = Quaternion.LookRotation(forward, groundNormal);
            Quaternion newRot = Quaternion.Slerp(rb.rotation, tiltRot, slopeAdjustSpeed * Time.fixedDeltaTime);

            Vector3 horizontalDir = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (horizontalDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalDir, groundNormal);
                newRot = Quaternion.Slerp(newRot, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }

            rb.MoveRotation(newRot);
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
        Vector3 currentHorVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 gravityAlongSlope = Vector3.ProjectOnPlane(Physics.gravity, groundNormal);      
        Vector3 targetVel = dir * (sprintAction.IsPressed() ? runSpeed : walkSpeed);

        //Debug.Log(rb.linearVelocity);

        force = ((targetVel - currentHorVel) * acceleration) - gravityAlongSlope;
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