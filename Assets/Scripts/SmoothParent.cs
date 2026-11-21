using UnityEngine;
using NaughtyAttributes;

public class SmoothParent : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private bool follow = true;
    [ShowIf("follow")][SerializeField] private float followSpeed = 5f;

    [Header("Rotation")]
    [SerializeField] private bool rotate = true;
    [ShowIf("rotate")][SerializeField] private float rotateSpeed = 5f;

    [Header("Offset")]
    [SerializeField] private bool keepPositionOffset = true;
    [SerializeField] private bool keepRotationOffset = true;

    private Vector3 positionOffset;
    private Quaternion rotationOffset;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target no asignado en SmoothParent");
            return;
        }

        if (keepPositionOffset)
            positionOffset = Quaternion.Inverse(target.rotation) * (transform.position - target.position);

        if (keepRotationOffset)
            rotationOffset = Quaternion.Inverse(target.rotation) * transform.rotation;
    }

private void LateUpdate()
{
    if (target == null) return;

    // Posición
    if (follow)
    {
        Vector3 targetPosition = target.position + (keepPositionOffset ? target.rotation * positionOffset : Vector3.zero);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    // Rotación
    if (rotate)
    {
        Quaternion targetRotation = keepRotationOffset ? target.rotation * rotationOffset : target.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }
}

    public void ToggleFollow(bool value) => follow = value;
    public void ToggleRotation(bool value) => rotate = value;
}