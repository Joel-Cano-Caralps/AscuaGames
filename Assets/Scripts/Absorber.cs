using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{
    [SerializeField] private float absorbeForce = 10f;

    [SerializeField] private float maxAbsorbTime = 2f;

    private readonly List<Rigidbody> targets = new();
    private readonly Dictionary<Rigidbody, float> timeInside = new();

    private int interactableLayer;

    void Awake() => interactableLayer = LayerMask.NameToLayer("Interactable");

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == interactableLayer && other.attachedRigidbody != null
            && !targets.Contains(other.attachedRigidbody))
        {
            targets.Add(other.attachedRigidbody);
            timeInside[other.attachedRigidbody] = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == interactableLayer && other.attachedRigidbody != null)
        {
            targets.Remove(other.attachedRigidbody);
            timeInside.Remove(other.attachedRigidbody);
        }
    }

    void FixedUpdate()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = targets[i];
            if (rb == null) { targets.RemoveAt(i); continue; }

            float t = timeInside[rb] + Time.fixedDeltaTime;
            timeInside[rb] = t;
            if (t > maxAbsorbTime) continue; // stayed too long: stop pulling it

            Vector3 dir = transform.position - rb.position;
            rb.AddForce(dir.normalized * absorbeForce, ForceMode.Acceleration);
        }
    }
}
