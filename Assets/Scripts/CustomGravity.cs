using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [Tooltip("1 = normal Earth gravity (-9.81). Higher = falls faster.")]
    public float gravityScale = 2.5f;

    [Tooltip("Optional: Set a custom pull direction (default is straight down).")]
    public Vector3 gravityDirection = Vector3.down;

    private Rigidbody rb;

    public int positionScore;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Turn off Unity's built-in gravity so we have full control
        rb.useGravity = false;
        positionScore = Random.Range(0, 3);
    }

    void FixedUpdate()
    {
        // Apply custom gravity independent of the ball's mass
        Vector3 gravity = gravityDirection.normalized * (9.81f * gravityScale);
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
}