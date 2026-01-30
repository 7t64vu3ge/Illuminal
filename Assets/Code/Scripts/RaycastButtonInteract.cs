using UnityEngine;

public class RaycastButtonInteract : MonoBehaviour
{
    public Transform rayOrigin;
    public float rayDistance = 3f;
    public LayerMask buttonLayer;

    private RotateButton currentButton;

    void Awake()
    {
        // Auto-assign if not set in Inspector
        if (rayOrigin == null)
            rayOrigin = Camera.main.transform;
    }

    void Update()
    {
        if (rayOrigin == null) return;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, rayDistance, buttonLayer))
        {
            currentButton = hit.collider.GetComponent<RotateButton>();

            if (currentButton != null && Input.GetKeyDown(KeyCode.E))
            {
                currentButton.RotateOnce();
            }
        }
        else
        {
            currentButton = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!rayOrigin) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * rayDistance);
    }
}
