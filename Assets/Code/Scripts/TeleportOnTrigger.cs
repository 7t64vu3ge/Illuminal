using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Vector3 teleportPosition = new Vector3(11.2f, -24.5f, -42.53f);

    [System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = teleportPosition;
            rb.rotation = Quaternion.identity;
        }
        else
        {
            other.transform.position = teleportPosition;
        }
    }
}
