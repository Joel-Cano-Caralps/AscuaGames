using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Walking, Running }
    public PlayerState CurrentState => currentState;

    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float fallForce = 9.81f;
    [SerializeField] float maxSlopeAngle = 45f;

    [Header("Ground Check")]
    [SerializeField] float groundCheckDistance = 0.3f;
    [SerializeField] float groundCheckRadius = 0.25f;
    private Vector3 groundSpherePos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private ParticleSystem movementParticles;
    [SerializeField] private GameObject stunBirds;
    InputAction moveAction;
    InputAction sprintAction;
    Rigidbody rb;
    Transform cam;
    Animator animator;
    private Grab playerGrabScript;
    PlayerState currentState = PlayerState.Idle;
    Vector3 lastMoveDir;
    public bool IsStunned => !controlEnabled;
    Coroutine stunRoutine;
    Vector3 force;
    Vector3 groundNormal = Vector3.up;
    private bool canMove, tooSteep, controlEnabled = true;
    Vector3 groundRayOrigin => transform.position + Vector3.up * 0.2f;
    float groundRayLength => 0.2f + groundCheckDistance;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
        playerGrabScript = GetComponent<Grab>();
        moveAction = inputActions.FindAction("Move");
        sprintAction = inputActions.FindAction("Sprint");

        lastMoveDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        groundSpherePos = transform.position;
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
        if (!controlEnabled)
        {
            currentState = PlayerState.Idle;
            //FRENAR EN HORIZONTAL (MANTENER LA VELOCIDAD VERTICAL PARA QUE CAIGA)
            Vector3 vel = rb.linearVelocity;
            Vector3 horizontal = Vector3.ProjectOnPlane(vel, Vector3.up);
            Vector3 brake = -horizontal * acceleration;
            rb.AddForce(brake, ForceMode.Acceleration);
            if (!IsGrounded()) rb.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);
            return;
        }

        Vector2 input = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);

        Vector3 dir = GetMoveDirection(input);
        bool grounded = IsGrounded();

        if (dir.sqrMagnitude > 0.01f) lastMoveDir = dir.normalized;

        Rotate();

        //SI LA PENDIENTE SUPERA EL MAXIMO, NO SE PUEDE CONTROLAR AL PERSONAJE
        tooSteep = Vector3.Angle(Vector3.up, groundNormal) > maxSlopeAngle;
        canMove = grounded && !tooSteep;

        if (canMove) Move(dir);
        else if (!grounded) rb.AddForce(Vector3.down * fallForce, ForceMode.Acceleration);

        bool isMoving = canMove && dir.sqrMagnitude > 0.01f;
        
        if (!isMoving)
        {
            movementParticles.Stop();
            currentState = PlayerState.Idle;
        }
        else
        {
            movementParticles.Play();
            currentState = sprintAction.IsPressed() ? PlayerState.Running : PlayerState.Walking;
        }
    }

    void Update()
    {
        Debug.Log("Grounded " + IsGrounded());
        SetAnimations();
    }

    Vector3 GetMoveDirection(Vector2 input)
    {
        Vector3 forward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
        return forward * input.y + right * input.x;
    }
    
    void Rotate()
    {
        //NORMAL DEL SUELO SUAVIZADA (SI NO HAY SUELO, VUELVE A "ARRIBA")
        Vector3 rawNormal = Vector3.up;
        if (Physics.Raycast(groundRayOrigin, Vector3.down, out RaycastHit hit, groundRayLength))
            rawNormal = hit.normal;

        float t = 1f - Mathf.Exp(-rotationSpeed * Time.fixedDeltaTime);
        groundNormal = Vector3.Slerp(groundNormal, rawNormal, t).normalized;

        //MIRAR HACIA lastMoveDir PERO INCLINADO SEGUN EL SUELO
        Vector3 slopeForward = Vector3.ProjectOnPlane(lastMoveDir, groundNormal);
        Quaternion target = Quaternion.LookRotation(slopeForward, groundNormal);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, t));
    }


    void Move(Vector3 dir)
    {
        //DIRECCION Y VELOCIDAD ACTUAL PROYECTADAS EN EL MISMO PLANO (LA RAMPA)
        Vector3 currentSlopeDir = Vector3.ProjectOnPlane(dir, groundNormal).normalized * dir.magnitude;
        Vector3 currentSlopeVel = Vector3.ProjectOnPlane(rb.linearVelocity, groundNormal);
        Vector3 finalVel = currentSlopeDir * (sprintAction.IsPressed() ? runSpeed : walkSpeed);
        Vector3 gravityAlongSlope = Vector3.ProjectOnPlane(Physics.gravity, groundNormal);

        force = (finalVel - currentSlopeVel) * acceleration - gravityAlongSlope;
        rb.AddForce(force, ForceMode.Acceleration);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force);
        Gizmos.DrawSphere(transform.position + force, 0.05f);
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    void SetAnimations()
    {
        bool idle = currentState == PlayerState.Idle;
        animator.SetBool("isMoving", !idle);
        animator.SetBool("isRunning", currentState == PlayerState.Running);
    }



    //PUNTO DE ENTRADA UNICO: SI YA ESTABA ATURDIDO, REINICIA EL TIEMPO (NO SE SOLAPAN)
    public void Stun(float time, bool knockOut = false)
    {
        //Debug.Log("STUNNED!");
        //Debug.Log("KNOCKOUT:" + knockOut);
        if (stunRoutine != null) StopCoroutine(stunRoutine);
        //BLOQUEO MOVIMIENTO + ANIMACIONES
        stunRoutine = StartCoroutine(StunPlayer(time,knockOut));
        //DROP ITEMS
        if(knockOut){StartCoroutine(playerGrabScript.Release(0f,.1f,.1f));}
    }

    public IEnumerator StunPlayer(float time, bool animation = false)
    {
        controlEnabled = false;
        if(animation)
        {
            animator.SetBool("stunned", true);
            animator.SetTrigger("stun");
            stunBirds.SetActive(true);
        }


        yield return new WaitForSeconds(time);

        if(animation)
        {
            stunBirds.SetActive(false);
            animator.SetBool("stunned", false);
        }

        controlEnabled = true;
        if(animation)
        stunRoutine = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(groundSpherePos, groundCheckRadius);
    }

    public void SetInputAction(InputActionAsset inAct)
    {
        inputActions = inAct;
        moveAction = inputActions.FindAction("Move");
        sprintAction = inputActions.FindAction("Sprint");
        moveAction.Enable();
        sprintAction.Enable();
    }
}
