using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    private Animator animator;
    [SerializeField] private float pickupCooldown = 0.5f;
    private float nextPickupTime;
    [SerializeField] private ParticleSystem grabParticles;
    private int heldOriginalLayer;
    private PlayerController playerController;
    private GameObject currentGrabGO;
    private Rigidbody heldRb;
    [SerializeField] Volume volume;



    int interactableLayer, ignorePlayerLayer;
    bool releasing;

    void Awake()
    {
        grabAction = inputActions.FindAction("Throw");
        shootAction = inputActions.FindAction("Shoot");
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        interactableLayer = LayerMask.NameToLayer("Interactable");
        ignorePlayerLayer = LayerMask.NameToLayer("IgnorePlayer");

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
        if (other.gameObject.layer == interactableLayer)
            currentGrabGO = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == interactableLayer)
            currentGrabGO = null;
    }

    void Update()
    {
        if (heldRb != null)
        {
            if (releasing) return;
            //CLICK IZQ O CONTROL
            if (grabAction.WasPressedThisFrame())
            {
                StartCoroutine(Release(throwAngleAmount, throwForce, 0.6f));
                playerController.Stun(1f);
                animator.SetTrigger("throw");
                GetComponent<PlayerController>().SetInputAction(inputActions);

            }
            //CLICK DER O ESPACIO
            else if (shootAction.WasPressedThisFrame())
            {
                StartCoroutine(Release(shootAngleAMount, shootForce, 0.6f));
                playerController.Stun(1f);
                animator.SetTrigger("shoot");
                GetComponent<PlayerController>().SetInputAction(inputActions);

            }
            return;
        }

        if (currentGrabGO == null || Time.time < nextPickupTime) return;

        if (!currentGrabGO.TryGetComponent(out heldRb))
        {
            currentGrabGO = null;
            return;
        }
        grabParticles.Play();
        animator.SetTrigger("grab");
        heldOriginalLayer = heldRb.gameObject.layer;
        SetLayerRecursive(heldRb.gameObject, ignorePlayerLayer);

        if (heldRb.tag == "Dizzy")
        {
            playerController.SetInputAction(DizzyInputActions);
        }
    }

    public IEnumerator Release(float angleForce, float force, float waitTime)
    {
        if (heldRb == null) yield break;

        releasing = true;
        grabParticles.Stop();

        yield return new WaitForSeconds(waitTime);

        // Another Release may have already dropped it during the wait
        if (heldRb != null)
        {
            SetLayerRecursive(heldRb.gameObject, heldOriginalLayer);
            Vector3 dir = (grabPos.forward + Vector3.up * angleForce).normalized;
            heldRb.linearVelocity = dir * force;
            heldRb = null;
            currentGrabGO = null;
            nextPickupTime = Time.time + pickupCooldown;
        }

        releasing = false;
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
