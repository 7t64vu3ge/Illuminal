using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward,
                                 out RaycastHit hit, interactDistance))
            {
                hit.collider.GetComponent<TV_transition>()?.Interact();
            }
        }
    }
}
