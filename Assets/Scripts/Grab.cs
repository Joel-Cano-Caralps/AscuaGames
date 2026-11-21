using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;
    [SerializeField] InputActionAsset DizzyInputActions;
    InputAction grabAction;
    InputAction shootAction;
    [SerializeField] private float grabStrength = 10f;
    [SerializeField] private Transform grabPos;
    [SerializeField] private float throwForce = 8f, shootForce = 10f;
    [SerializeField] private float throwAngleAmount = 0.3f, shootAngleAMount;

    [SerializeField] private float pickupCooldown = 0.5f;
    private float nextPickupTime;

    private int heldOriginalLayer;

    private GameObject currentGrabGO;
    private Rigidbody heldRb;

    void Awake()
    {
        grabAction = inputActions.FindAction("Throw");
        shootAction = inputActions.FindAction("Shoot");
    }

    void OnEnable()
    {
        grabAction.Enable();
        shootAction.Enable();
    }

    void OnDisable()
    {
        grabAction.Disable();
        shootAction.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            currentGrabGO = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            currentGrabGO = null;
    }

    void Update()
    {
        // LANZAMIENTO
        if (heldRb != null)
        {
            if (grabAction.WasPressedThisFrame())
            {
                Release(throwAngleAmount, throwForce);
                GetComponent<PlayerController>().SetInputAction(inputActions);
                return;  
            }
            else if (shootAction.WasPressedThisFrame())
            {
                Release(shootAngleAMount, shootForce);
                GetComponent<PlayerController>().SetInputAction(inputActions);
                return;
            }
            return;
        }

        if (currentGrabGO != null && Time.time >= nextPickupTime)
        {
            heldRb = currentGrabGO.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldOriginalLayer = heldRb.gameObject.layer;
                SetLayerRecursive(heldRb.gameObject, LayerMask.NameToLayer("IgnorePlayer"));

                if (heldRb.tag == "Dizzy")
                {
                    
                    GetComponent<PlayerController>().SetInputAction(DizzyInputActions);
                }
            }
        }
    }

    void Release(float angleForce, float force)
    {
        SetLayerRecursive(heldRb.gameObject, heldOriginalLayer);

        Vector3 dir = (grabPos.forward + Vector3.up * angleForce).normalized;
        heldRb.linearVelocity = dir * force;
        heldRb = null;
        currentGrabGO = null;
        nextPickupTime = Time.time + pickupCooldown;
    }

    static void SetLayerRecursive(GameObject go, int layer)
    {
        foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = layer;
    }

    void FixedUpdate()
    {
        if (heldRb == null) return;

        Vector3 toTarget = grabPos.position - heldRb.position;
        heldRb.linearVelocity = toTarget * grabStrength;
    }
}
