using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private bool lockX, lockY, lockZ;

    void Update()
    {
        Vector3 axis = new Vector3(
            lockX ? 0f : 1f,
            lockY ? 0f : 1f,
            lockZ ? 0f : 1f);

        transform.Rotate(axis * rotationSpeed * Time.deltaTime);
    }
}
