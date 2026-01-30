using UnityEngine;

public class SlidingDoor : MonoBehaviour, IInteractable
{
    public enum SlideDirection
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Backward
    }

    [Header("Movement Settings")]
    public SlideDirection direction;
    public float distance = 2f;
    public float speed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen;
    private bool isMoving;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + GetDirectionVector() * distance;
    }

    Vector3 GetDirectionVector()
    {
        switch (direction)
        {
            case SlideDirection.Left: return -transform.right;
            case SlideDirection.Right: return transform.right;
            case SlideDirection.Up: return transform.up;
            case SlideDirection.Down: return -transform.up;
            case SlideDirection.Forward: return transform.forward;
            case SlideDirection.Backward: return -transform.forward;
            default: return Vector3.zero;
        }
    }

    public void Interact()
    {
        // 🔑 Allow reversal mid-movement
        isOpen = !isOpen;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? openPos : closedPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            transform.position = target;
            isMoving = false;
        }
    }
}
