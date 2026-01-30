using UnityEngine;

public class HallIllusionController : MonoBehaviour
{
    [Header("References")]
    public Transform player;   // Player transform
    public Transform hall;     // This is your "Hall" parent
    

    //internal varibales 
    int currentBlockIndex = -1;
    float lastPlayerX;



    void Start()
    {   

        if (player == null)
        {
            Debug.LogError("HallIllusionController: Player NOT assigned");
            return;
        }

        if (hall == null)
        {
            Debug.LogError("HallIllusionController: Hall NOT assigned");
            return;
        }

        Debug.Log("Player detected: " + player.name);
        Debug.Log("Hall detected: " + hall.name);
        Debug.Log("Number of hall blocks: " + hall.childCount);
        lastPlayerX = player.position.x;

    }
    void Update()
    {
        // Step 1: detect which block player is on
        int newIndex = GetCurrentBlockIndex();

        if (newIndex != currentBlockIndex)
        {
            currentBlockIndex = newIndex;
            Debug.Log("Now standing on HallBlock index: " + currentBlockIndex);
        }

       
    }


    int GetCurrentBlockIndex()
        {
            float playerX = player.position.x;

            for (int i = 0; i < hall.childCount; i++)
            {
                Transform block = hall.GetChild(i);

                Renderer r = block.GetComponentInChildren<Renderer>();
                if (r == null) continue;

                float halfWidth = r.bounds.size.x * 0.5f;
                float leftEdge = block.position.x - halfWidth;
                float rightEdge = block.position.x + halfWidth;

                if (playerX >= leftEdge && playerX <= rightEdge)
                {
                    return i;
                }
            }

            return -1; // Player not on any block
        }

}
