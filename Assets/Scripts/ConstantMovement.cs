using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    public enum MoveDirection { Left, Right, Up, Down }

    [SerializeField] MoveDirection direction = MoveDirection.Right;
    [SerializeField] float speed = 2f;
    [SerializeField] private bool startOver = false;
    private Vector3 startPos;
    void Awake()
    {
        startPos = transform.localPosition;
    }
    void Update()
    {
        transform.Translate(GetVector() * (speed * Time.deltaTime), Space.World);
    }

    void Onable()
    {
        if(startOver){transform.position = startPos;}
    }

    Vector3 GetVector()
    {
        switch (direction)
        {
            case MoveDirection.Left: return Vector3.left;
            case MoveDirection.Right: return Vector3.right;
            case MoveDirection.Up: return Vector3.up;
            case MoveDirection.Down: return Vector3.down;
            default: return Vector3.zero;
        }
    }
}
