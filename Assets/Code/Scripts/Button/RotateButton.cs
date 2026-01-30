using UnityEngine;
using System.Collections;

public class RotateButton : MonoBehaviour
{
    [Header("Rotation Targets")]
    public Transform[] targets;

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationAngle = 90f;
    public float rotationSpeed = 180f;

    private bool isRotating;

    public void RotateOnce()
    {
        if (isRotating) return;
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        isRotating = true;

        float rotated = 0f;

        while (rotated < rotationAngle)
        {
            float step = rotationSpeed * Time.deltaTime;
            step = Mathf.Min(step, rotationAngle - rotated);

            foreach (Transform t in targets)
            {
                if (t != null)
                    t.Rotate(rotationAxis, step, Space.Self);
            }

            rotated += step;
            yield return null;
        }

        isRotating = false;
    }
}
