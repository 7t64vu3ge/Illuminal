using UnityEngine;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [Header("Objects this button activates")]
    public MonoBehaviour[] targets;

    public void Interact()
    {
        foreach (MonoBehaviour mb in targets)
        {
            if (mb is IInteractable interactable)
            {
                interactable.Interact();
            }
        }
    }
}
