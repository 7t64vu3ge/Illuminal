// using UnityEngine;

// public class PerspectiveScaler : MonoBehaviour
// {
//     public float grabDistance = 10f;
//     public float maxPlaceDistance = 100f;
//     public LayerMask placementMask;

//     private GameObject heldObject;
//     private Rigidbody heldRb;
//     private Collider heldCollider;
//     private Renderer heldRenderer;

//     private float originalDistance;
//     private Vector3 originalScale;

//     private bool isScalable;

//     [Header("Damping")]
//     public float positionDampTime = 0.05f;
//     public float scaleDampSpeed = 15f;

//     private Vector3 positionVelocity;

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             TryGrab();
//         }

//         if (Input.GetMouseButtonUp(0) && heldObject != null)
//         {
//             PlaceObject();
//         }

//         if (heldObject != null)
//         {
//             HoldObject();
//         }
//     }

//     void TryGrab()
//     {
//         if (heldObject != null) return;

//         Ray ray = new Ray(transform.position, transform.forward);

//         if (!Physics.SphereCast(ray, 0.4f, out RaycastHit hit, grabDistance))
//             return;

//         if (!hit.collider.CompareTag("Scalable") &&
//             !hit.collider.CompareTag("Interactable"))
//             return;

//         heldObject = hit.collider.gameObject;
//         heldRb = heldObject.GetComponent<Rigidbody>();
//         heldCollider = heldObject.GetComponent<Collider>();

//         isScalable = hit.collider.CompareTag("Scalable");

//         if (isScalable)
//         {
//             heldRenderer = heldObject.GetComponent<Renderer>();
//             originalDistance = Vector3.Distance(transform.position, heldObject.transform.position);
//             originalScale = heldObject.transform.localScale;
//         }
//         else
//         {
//             heldRenderer = null; // explicitly disable scaling path
//         }

//         heldRb.isKinematic = true;
//         heldCollider.enabled = false;
//     }

//     void HoldObject()
//     {
//         Ray ray = new Ray(transform.position, transform.forward);
//         bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, maxPlaceDistance, placementMask);

//         if (isScalable)
//         {
//             Vector3 startPos = heldObject.transform.position;
//             Vector3 startScale = heldObject.transform.localScale;

//             float finalDistance = hitSomething
//                 ? Vector3.Distance(transform.position, hit.point)
//                 : grabDistance;

//             for (int i = 0; i < 10; i++)
//             {
//                 float scaleFactor = finalDistance / originalDistance;
//                 Vector3 newScale = Vector3.ClampMagnitude(originalScale * scaleFactor, 20f);
//                 heldObject.transform.localScale = newScale;

//                 Vector3 finalPos = transform.position + transform.forward * finalDistance;
//                 heldObject.transform.position = finalPos;

//                 if (!Physics.CheckBox(
//                         heldRenderer.bounds.center,
//                         heldRenderer.bounds.extents * 0.95f,
//                         Quaternion.identity,
//                         placementMask,
//                         QueryTriggerInteraction.Ignore))
//                     break;

//                 finalDistance *= 0.9f;
//             }

//             Vector3 targetPos = heldObject.transform.position;
//             Vector3 targetScale = heldObject.transform.localScale;

//             heldObject.transform.position = startPos;
//             heldObject.transform.localScale = startScale;

//             heldObject.transform.position = Vector3.SmoothDamp(
//                 startPos, targetPos, ref positionVelocity, positionDampTime);

//             heldObject.transform.localScale = Vector3.Lerp(
//                 startScale, targetScale, Time.deltaTime * scaleDampSpeed);
//         }
//         else
//         {
//             // Interactable: translate only
//             Vector3 targetPos = hitSomething
//                 ? hit.point
//                 : transform.position + transform.forward * grabDistance;

//             heldObject.transform.position = Vector3.SmoothDamp(
//                 heldObject.transform.position,
//                 targetPos,
//                 ref positionVelocity,
//                 positionDampTime);
//         }
//     }

//     void PlaceObject()
//     {
//         heldCollider.enabled = true;
//         heldRb.isKinematic = false;

//         heldObject = null;
//         heldCollider = null;
//         heldRb = null;
//         heldRenderer = null;
//         isScalable = false;
//     }
// }


using UnityEngine;

public class PerspectiveScaler : MonoBehaviour
{
    public float grabDistance = 10f;
    public float maxPlaceDistance = 100f;
    public LayerMask placementMask;

    private GameObject heldObject;
    private Rigidbody heldRb;
    private Collider heldCollider;
    private Renderer heldRenderer;

    private float originalDistance;
    private Vector3 originalScale;

    private bool isScalable;

    [Header("Damping")]
    public float positionDampTime = 0.05f;
    public float scaleDampSpeed = 15f;

    private Vector3 positionVelocity;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryGrab();

        if (Input.GetMouseButtonUp(0) && heldObject != null)
            PlaceObject();

        if (heldObject != null)
            HoldObject();
    }

    void TryGrab()
    {
        if (heldObject != null) return;

        Ray ray = new Ray(transform.position, transform.forward);

        if (!Physics.SphereCast(ray, 0.4f, out RaycastHit hit, grabDistance))
            return;

        if (!hit.collider.CompareTag("Scalable") &&
            !hit.collider.CompareTag("Interactable"))
            return;

        heldObject = hit.collider.gameObject;
        heldRb = heldObject.GetComponent<Rigidbody>();
        heldCollider = heldObject.GetComponent<Collider>();

        isScalable = hit.collider.CompareTag("Scalable");

        if (isScalable)
        {
            heldRenderer = heldObject.GetComponent<Renderer>();
            originalDistance = Vector3.Distance(transform.position, heldObject.transform.position);
            originalScale = heldObject.transform.localScale;
        }
        else
        {
            heldRenderer = heldObject.GetComponent<Renderer>(); // needed for bounds offset
        }

        heldRb.isKinematic = true;
        heldCollider.enabled = false;
    }

    void HoldObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, maxPlaceDistance, placementMask);

        if (isScalable)
        {
            Vector3 startPos = heldObject.transform.position;
            Vector3 startScale = heldObject.transform.localScale;

            float finalDistance = hitSomething
                ? Vector3.Distance(transform.position, hit.point)
                : grabDistance;

            for (int i = 0; i < 10; i++)
            {
                float scaleFactor = finalDistance / originalDistance;
                Vector3 newScale = Vector3.ClampMagnitude(originalScale * scaleFactor, 20f);
                heldObject.transform.localScale = newScale;

                Vector3 finalPos = transform.position + transform.forward * finalDistance;
                heldObject.transform.position = finalPos;

                if (!Physics.CheckBox(
                        heldRenderer.bounds.center,
                        heldRenderer.bounds.extents * 0.95f,
                        Quaternion.identity,
                        placementMask,
                        QueryTriggerInteraction.Ignore))
                    break;

                finalDistance *= 0.9f;
            }

            Vector3 targetPos = heldObject.transform.position;
            Vector3 targetScale = heldObject.transform.localScale;

            heldObject.transform.position = startPos;
            heldObject.transform.localScale = startScale;

            heldObject.transform.position = Vector3.SmoothDamp(
                startPos, targetPos, ref positionVelocity, positionDampTime);

            heldObject.transform.localScale = Vector3.Lerp(
                startScale, targetScale, Time.deltaTime * scaleDampSpeed);
        }
        else
        {
            // ✅ FIXED: Interactable translate-only with bounds offset
            Vector3 targetPos;

            if (hitSomething)
            {
                Vector3 extents = heldRenderer.bounds.extents;

                float offset =
                    Mathf.Abs(hit.normal.x) * extents.x +
                    Mathf.Abs(hit.normal.y) * extents.y +
                    Mathf.Abs(hit.normal.z) * extents.z;

                targetPos = hit.point + hit.normal * offset;
            }
            else
            {
                targetPos = transform.position + transform.forward * grabDistance;
            }

            heldObject.transform.position = Vector3.SmoothDamp(
                heldObject.transform.position,
                targetPos,
                ref positionVelocity,
                positionDampTime);
        }
    }

    void PlaceObject()
    {
        heldCollider.enabled = true;
        heldRb.isKinematic = false;

        heldObject = null;
        heldCollider = null;
        heldRb = null;
        heldRenderer = null;
        isScalable = false;
    }
}
