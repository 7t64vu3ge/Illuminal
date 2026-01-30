using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationAngle = 90f;
    public float rotationSpeed = 180f;

    [Tooltip("Local axis to rotate around")]
    public Vector3 rotationAxis = Vector3.up;

    private Quaternion targetRotation;
    private bool isRotating;

    void Start()
    {
        targetRotation = transform.rotation;
        rotationAxis.Normalize();
    }

    // Called externally by button
    public void Rotate()
    {
        targetRotation *= Quaternion.AngleAxis(rotationAngle, rotationAxis);
        isRotating = true;
    }

    void Update()
    {
        if (!isRotating) return;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }
}
