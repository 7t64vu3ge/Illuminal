using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask doorLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, doorLayer))
            {
                Door door = hit.collider.GetComponentInParent<Door>();
                if (door != null)
                {
                    door.ToggleDoor();
                }
            }
        }
    }
}
