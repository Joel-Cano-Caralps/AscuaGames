using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;
    InputAction grabAction;

    [SerializeField] private float grabStrength = 10f;
    [SerializeField] private Transform grabPos;

    [SerializeField] private string heldLayerName = "Held";
    private int heldOriginalLayer;

    private GameObject currentGrabGO;
    private Rigidbody heldRb;

    void Awake()
    {
        grabAction = inputActions.FindAction("Grab/Throw");
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
        if (!grabAction.IsPressed())
        {
            if (heldRb != null) Release();
            return;
        }

        if (heldRb == null && currentGrabGO != null)
        {
            heldRb = currentGrabGO.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldOriginalLayer = heldRb.gameObject.layer;
                SetLayerRecursive(heldRb.gameObject, LayerMask.NameToLayer(heldLayerName));
            }
        }
    }

    void Release()
    {
        SetLayerRecursive(heldRb.gameObject, heldOriginalLayer);
        heldRb = null;
    }

    static void SetLayerRecursive(GameObject go, int layer)
    {
        foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = layer;
    }

    void FixedUpdate()
    {
        if (heldRb == null) return;

        // Pull the object toward grabPos with the given strength
        Vector3 toTarget = grabPos.position - heldRb.position;
        heldRb.linearVelocity = toTarget * grabStrength;
    }
}
