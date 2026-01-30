using UnityEngine;

public class ProximityRotateBlock : MonoBehaviour
{
    [Header("Target")]
    public Transform player;          // Drag Player here

    [Header("Distance Settings")]
    public float triggerDistance = 5f; // Manually set

    [Header("Rotation Settings")]
    public float startAngle = 0f;
    public float endAngle = -44f;

    private Quaternion startRotation;
    private Quaternion endRotation;

    void Start()
    {
        startRotation = Quaternion.Euler(startAngle, 0f, 0f);
        endRotation   = Quaternion.Euler(endAngle, 0f, 0f);
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector3.Distance(player.position, transform.position);

        // 1️⃣ Distance check
        if (distance <= triggerDistance)
        {
            // 2️⃣ Interpolation based on distance
            float t = 1f - (distance / triggerDistance);
            t = Mathf.Clamp01(t);

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
        }
        else
        {
            // Optional: reset when player goes away
            transform.rotation = startRotation;
        }
    }
}
