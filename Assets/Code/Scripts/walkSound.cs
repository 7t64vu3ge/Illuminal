using UnityEngine;
using System.Collections;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource footstepSource;
    public float stepInterval = 0.45f;

    private Coroutine stepRoutine;

    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
            StartSteps();
        else
            StopSteps();
    }

    void StartSteps()
    {
        if (stepRoutine != null) return;
        stepRoutine = StartCoroutine(FootstepLoop());
    }

    void StopSteps()
    {
        if (stepRoutine == null) return;

        StopCoroutine(stepRoutine);
        stepRoutine = null;

        if (footstepSource.isPlaying)
            footstepSource.Stop();
    }

    IEnumerator FootstepLoop()
    {
        while (true)
        {
            if (!footstepSource.isPlaying)
                footstepSource.Play();

            yield return new WaitForSeconds(stepInterval);
        }
    }
}
