using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;
    InputAction grabAction;

    [SerializeField] private float grabStrength = 10f;
    [SerializeField] private Transform grabPos;

    [SerializeField] private float throwForce = 8f;
    [SerializeField] private float upAmount = 0.3f;

    [SerializeField] private float pickupCooldown = 0.5f;
    private float nextPickupTime;

    private int heldOriginalLayer;

    private GameObject currentGrabGO;
    private Rigidbody heldRb;

    void Awake()
    {
        grabAction = inputActions.FindAction("Throw");
    }

    void OnEnable()
    {
        grabAction.Enable();
    }

    void OnDisable()
    {
        grabAction.Disable();
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

        if (heldRb != null)
        {
            if (grabAction.WasPressedThisFrame()) Release();
            return;
        }

        if (currentGrabGO != null && Time.time >= nextPickupTime)
        {
            heldRb = currentGrabGO.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldOriginalLayer = heldRb.gameObject.layer;
                SetLayerRecursive(heldRb.gameObject, LayerMask.NameToLayer("IgnorePlayer"));
            }
        }
    }

    void Release()
    {
        SetLayerRecursive(heldRb.gameObject, heldOriginalLayer);

        Vector3 dir = (grabPos.forward + Vector3.up * upAmount).normalized;
        heldRb.linearVelocity = dir * throwForce;
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
