using UnityEngine;
using UnityEngine.SceneManagement;
public class HallTeleport : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Pilot");
        }
    }
}
