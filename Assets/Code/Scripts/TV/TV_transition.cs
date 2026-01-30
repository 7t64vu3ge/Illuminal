using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TV_transition : MonoBehaviour
{
    [Header("Scene Transition")]
    public string nextSceneName = "Hall Copy";
    public float blackoutDelay = 2f;

    [Header("Unconscious Effect")]
    public float fallDelay = 0.2f;       
    public float soundDelay = 0.5f;       
    public float tiltAngle = 85f;            
    public float dropAmount = 0.6f;          
    public float effectDuration = 1.5f;      

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip faintSound;    

    [Header("References")]
    public FirstPersonController playerController; 

    private bool triggered = false;

    private void Awake()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<FirstPersonController>();
    }

    public void Interact()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        yield return StartCoroutine(UnconsciousEffect());

        if (ScreenFader.Instance != null)
            ScreenFader.Instance.FadeOut();

        yield return new WaitForSeconds(blackoutDelay);
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator UnconsciousEffect()
    {
        if (playerController == null) yield break;

        // --- LOCK MOVEMENT IMMEDIATELY ---
        playerController.cameraCanMove = false;
        playerController.playerCanMove = false;
        playerController.isWalking = false;
        playerController.enableHeadBob = false;

        Transform targetTransform = playerController.joint != null ? playerController.joint : playerController.playerCamera.transform;

        // --- VISUAL DELAY ---
        yield return new WaitForSeconds(fallDelay);

        // --- START FALLING ---
        Vector3 startPos = targetTransform.localPosition;
        Quaternion startRot = targetTransform.localRotation;

        Vector3 targetPos = startPos + (Vector3.down * dropAmount);
        Quaternion targetRot = startRot * Quaternion.Euler(0f, 0f, tiltAngle);

        float elapsed = 0f;
        bool soundPlayed = false;

        while (elapsed < effectDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / effectDuration);

            targetTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            targetTransform.localRotation = Quaternion.Slerp(startRot, targetRot, t);

            // --- SOUND DELAY LOGIC ---
            // This checks if we have reached the sound delay time
            if (!soundPlayed && elapsed >= soundDelay)
            {
                if (audioSource != null && faintSound != null)
                {
                    audioSource.PlayOneShot(faintSound);
                }
                soundPlayed = true;
            }

            yield return null;
        }

        targetTransform.localPosition = targetPos;
        targetTransform.localRotation = targetRot;

        yield return new WaitForSeconds(0.3f);
    }
}