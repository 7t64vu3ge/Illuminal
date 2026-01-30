using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class TransitionHallLoader : MonoBehaviour
{
    [Header("Scene Names")]
    public string pilotScene = "Pilot";
    public string room2Scene = "Room2";

    [Header("Unload Delay")]
    public float unloadDelay = 0.15f;

    private bool isLocked;       // prevents repeat loads
    private bool playerInside;  // tracks exit

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isLocked) return;

        isLocked = true;
        playerInside = true;

        StartCoroutine(HandleTransition());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Unlock ONLY after leaving the trigger
        playerInside = false;
        isLocked = false;
    }

    IEnumerator HandleTransition()
    {
        Scene active = SceneManager.GetActiveScene();

        string sceneToLoad;
        string sceneToUnload;

        if (active.name == pilotScene)
        {
            sceneToLoad = room2Scene;
            sceneToUnload = pilotScene;
        }
        else if (active.name == room2Scene)
        {
            sceneToLoad = pilotScene;
            sceneToUnload = room2Scene;
        }
        else
        {
            yield break;
        }

        // Load next scene additively
        AsyncOperation loadOp =
            SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!loadOp.isDone)
            yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        // Delay unload to avoid hitch stacking
        yield return new WaitForSeconds(unloadDelay);

        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
