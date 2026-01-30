using UnityEngine;

public class AutoCloseDoor : MonoBehaviour
{
    private bool closed = false;
    public GameObject Door1;
    public GameObject Door2;
    private Door door1,door2;
    private float t = 0.5f;
    Quaternion closedRot = Quaternion.Euler(0f, 0f, 0f);

    void Awake()
    {

        door1 = Door1.GetComponent<Door>();
        door2 = Door2.GetComponent<Door>();
    }
    void ToggleDoors()
    {
        door1.transform.localRotation = Quaternion.Slerp(door1.transform.localRotation, closedRot, t);
        door2.transform.localRotation = Quaternion.Slerp(door2.transform.localRotation, closedRot, t);
        StartCoroutine(door1.ToggleDoor());
        StartCoroutine(door2.ToggleDoor());
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !closed)
        {
            closed = true;
            ToggleDoors();
        }
    }
}
