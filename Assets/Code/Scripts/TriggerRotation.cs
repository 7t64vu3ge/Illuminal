using UnityEngine;

public class TriggerRotation : MonoBehaviour
{
 public GameObject player;
 private bool isTrack = false;
 private Vector3 colliderPos;
 public Vector3 posDifference;
private void Start() {
    // Debug.Log(this);
    colliderPos = this.transform.position;
    // Debug.Log(colliderPos);
}
private void OnTriggerEnter(Collider other)
{
    if(other.gameObject.tag == "Player")
    {
        isTrack = true;
    }
}

private void Update() {
    if (isTrack)
    {
        Vector3 playerPos = player.transform.position;
        posDifference =  playerPos - colliderPos ;
    }
}


}
