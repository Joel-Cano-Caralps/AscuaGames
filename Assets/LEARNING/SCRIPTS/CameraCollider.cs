using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private float movSpeed = 3f;
    [SerializeField] private float distOffset = 0.2f;
    [SerializeField] private LayerMask layerToIgnore;
    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        Transform pivot = transform.parent;
        Vector3 pivotPos = pivot.position;
        Vector3 idealPos = pivot.TransformPoint(startPos);
        Vector3 toIdeal = idealPos - pivotPos;

        Vector3 targetPos = idealPos;
        if (Physics.Raycast(pivotPos, toIdeal.normalized, out RaycastHit hit, toIdeal.magnitude, ~layerToIgnore))
        {
            targetPos = hit.point + hit.normal * distOffset;
        }

        float t = movSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPos, t);

        //Debug.Log($"Hit: {hit.collider != null} | Target: {targetPos} | Pos: {transform.position} | Name: {hit.collider.name}");
    }
}