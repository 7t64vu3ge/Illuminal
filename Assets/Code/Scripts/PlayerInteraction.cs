using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactLayer;

    private Camera playerCamera;

    [System.Obsolete]
    void Awake()
    {
        playerCamera = Camera.main ?? FindObjectOfType<Camera>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(interactKey)) return;

        TryInteract();
    }

    void TryInteract()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }
}
