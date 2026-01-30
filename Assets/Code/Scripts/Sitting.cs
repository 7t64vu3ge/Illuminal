using UnityEngine;
using System.Collections;

public class Sitting : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public float sitSpeed = 2f; // Lower this for a slower, smoother sit
    public KeyCode interactKey = KeyCode.F;

    [Header("References")]
    public Camera playerCamera;
    public FirstPersonController playerController; 

    private SitPoint currentSitPoint;
    private bool isSitting = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody playerRb;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (playerController == null)
            playerController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (!isSitting)
                TrySit();
            else
                StandUp();
        }
    }

    void TrySit()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            SitPoint sitPoint = hit.collider.GetComponent<SitPoint>();
            if (sitPoint != null)
            {
                currentSitPoint = sitPoint;
                originalPosition = transform.position;
                originalRotation = transform.rotation;
                StartCoroutine(MoveToSitPoint());
            }
        }
    }

    IEnumerator MoveToSitPoint()
    {
        isSitting = true;

        if (playerController != null)
        {
            playerController.playerCanMove = false;
            playerController.isWalking = false; 
            playerController.cameraCanMove = true; 
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        Vector3 targetPos = currentSitPoint.GetSitWorldPosition();
        Quaternion targetRot = Quaternion.LookRotation(currentSitPoint.transform.forward, Vector3.up);

        float elapsed = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        
        // We store the starting pitch to transition it to 0 (horizon) smoothly
        float startPitch = playerController.pitch;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * sitSpeed;
            
            // Smoothly move position
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed);
            
            // Smoothly rotate the body
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed);
            
            // UPDATE CONTROLLER INTERNALS EVERY FRAME
            // This prevents the "snap" because the controller stays in sync with the animation
            if (playerController != null)
            {
                playerController.yaw = transform.localEulerAngles.y;
                playerController.pitch = Mathf.Lerp(startPitch, 0f, elapsed);
                playerController.isWalking = false;
            }
            
            yield return null;
        }

        // Final snap to ensure perfect alignment
        transform.position = targetPos;
        transform.rotation = targetRot;
        
        if (playerController != null)
        {
            playerController.yaw = transform.localEulerAngles.y;
            playerController.pitch = 0f;
        }
    }

    void StandUp()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToStandPosition());
    }

    IEnumerator MoveToStandPosition()
    {
        float elapsed = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * sitSpeed;
            transform.position = Vector3.Lerp(startPos, originalPosition, elapsed);
            transform.rotation = Quaternion.Slerp(startRot, originalRotation, elapsed);
            
            // Keep the controller in sync while standing up too
            if (playerController != null)
            {
                playerController.yaw = transform.localEulerAngles.y;
            }
            
            yield return null;
        }

        transform.position = originalPosition;
        transform.rotation = originalRotation;

        if (playerRb != null)
            playerRb.isKinematic = false;

        if (playerController != null)
        {
            playerController.playerCanMove = true;
            playerController.yaw = transform.localEulerAngles.y;
        }

        currentSitPoint = null;
        isSitting = false;
    }
}