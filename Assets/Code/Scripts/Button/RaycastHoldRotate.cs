using UnityEngine;

public class RaycastHoldRotate : MonoBehaviour
{
    [Header("Raycast")]
    public Transform rayOrigin;          // usually camera
    public float rayDistance = 3f;
    public LayerMask buttonLayer;

    [Header("Rotation")]
    public Transform[] targets;
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 120f;

    private bool isHittingButton;

    void Update()
    {
        RaycastHit hit;
        isHittingButton = Physics.Raycast(
            rayOrigin.position,
            rayOrigin.forward,
            out hit,
            rayDistance,
            buttonLayer
        );

        if (isHittingButton && Input.GetKey(KeyCode.E))
        {
            RotateTargets();
        }
    }

    void RotateTargets()
    {
        float delta = rotationSpeed * Time.deltaTime;

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
                targets[i].Rotate(rotationAxis, delta, Space.World);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!rayOrigin) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * rayDistance);
    }
}