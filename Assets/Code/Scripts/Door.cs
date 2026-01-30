using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 3f; 
    private bool isOpenSound = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentCoroutine;
    private bool closeToDoor = false;

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && closeToDoor)
        {
            if(currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ToggleDoor());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            closeToDoor = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            closeToDoor = false;
        }
    }
    public IEnumerator ToggleDoor()
    {
        isOpenSound = !isOpenSound;

        if (isOpenSound)
        {
            audioSource.PlayOneShot(openSound);
        }
        else
        {
            audioSource.PlayOneShot(closeSound);
        }
        // ------ //
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;
        isOpen = !isOpen;
        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
       
    }

    // public void ToggleDoor()
    // {
    // }
}
