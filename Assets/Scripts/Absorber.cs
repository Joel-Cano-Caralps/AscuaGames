using System.Collections.Generic;
using UnityEngine;

public class Absorber : MonoBehaviour
{
    [SerializeField] private float absorbeForce = 10f;

    private readonly List<Rigidbody> targets = new();

    private int interactableLayer;

    void Awake() => interactableLayer = LayerMask.NameToLayer("Interactable");

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == interactableLayer && other.attachedRigidbody != null)
            targets.Add(other.attachedRigidbody);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == interactableLayer && other.attachedRigidbody != null)
            targets.Remove(other.attachedRigidbody);
    }

    void FixedUpdate()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] == null) { targets.RemoveAt(i); continue; }

            Vector3 dir = transform.position - targets[i].position;
            targets[i].AddForce(dir.normalized * absorbeForce, ForceMode.Acceleration);
        }
    }
}
