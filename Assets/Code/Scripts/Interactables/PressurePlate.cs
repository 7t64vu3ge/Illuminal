using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Plate Parts")]
    public Transform plateVisual;

    [Header("Plate Movement")]
    public float pressDepth = 0.2f;
    public float moveSpeed = 2f;

    [Header("Targets to activate")]
    public MonoBehaviour[] targets; // Must implement IInteractable

    private Vector3 startLocalPos;
    private Vector3 pressedLocalPos;

    private int validObjectsOnPlate;
    private bool isPressed;

    void Start()
    {
        if (plateVisual == null)
        {
            Debug.LogError("Plate Visual not assigned!");
            enabled = false;
            return;
        }

        startLocalPos = plateVisual.localPosition;
        pressedLocalPos = startLocalPos + Vector3.down * pressDepth;
    }

    void Update()
    {
        Vector3 target = isPressed ? pressedLocalPos : startLocalPos;

        plateVisual.localPosition = Vector3.MoveTowards(
            plateVisual.localPosition,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    void TriggerInteractables()
    {
        foreach (MonoBehaviour mb in targets)
        {
            if (mb is IInteractable interactable)
                interactable.Interact();
        }
    }

    bool IsValidActivator(Collider other)
    {
        return other.CompareTag("Player") || other.CompareTag("Scalable");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValidActivator(other)) return;

        validObjectsOnPlate++;

        if (!isPressed)
        {
            isPressed = true;
            TriggerInteractables(); // PRESS → open
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValidActivator(other)) return;

        validObjectsOnPlate--;

        if (validObjectsOnPlate <= 0)
        {
            validObjectsOnPlate = 0;
            isPressed = false;
            TriggerInteractables(); // RELEASE → close
        }
    }
}
