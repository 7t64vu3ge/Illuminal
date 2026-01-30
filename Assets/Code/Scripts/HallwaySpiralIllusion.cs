using UnityEngine;

public class HallwaySpiralIllusion : MonoBehaviour
{
    [Header("Hall References")]
    public Transform hallParent;          // Parent object named "Hall"
    public int triggerBlockIndex = 1;     // HallBlock (1)

    [Header("Scaling")]
    public float scaleSpeed = 0.5f;        // How fast the hall stretches
    public float maxExtraScaleX = 20f;     // Max added X scale

    [Header("Spacing")]
    public float spacingMultiplier = 2f;   // Push distance = 2 * size increase

    [Header("Rotation (Spiral Effect)")]
    public float rotationSpeed = 30f;      // Degrees per unit distance
    public float rotationOffsetPerBlock = 15f;

    private Transform[] hallBlocks;
    private float initialBlockSizeX;
    private bool illusionActive;

    void Start()
    {
        int count = hallParent.childCount;
        hallBlocks = new Transform[count];

        for (int i = 0; i < count; i++)
            hallBlocks[i] = hallParent.GetChild(i);

        initialBlockSizeX = hallBlocks[0].localScale.x;
    }

    void Update()
    {
        if (!illusionActive)
            CheckTrigger();

        if (illusionActive)
            UpdateHallway();
    }

    void CheckTrigger()
    {
        Transform triggerBlock = hallBlocks[triggerBlockIndex];

        float blockStartX = triggerBlock.position.x - (triggerBlock.localScale.x / 2f);
        float blockMidX = blockStartX + (triggerBlock.localScale.x / 2f);

        if (transform.position.x >= blockMidX)
            illusionActive = true;
    }

    void UpdateHallway()
    {
        float playerProgress = transform.position.x - hallBlocks[0].position.x;
        float extraScaleX = Mathf.Clamp(playerProgress * scaleSpeed, 0f, maxExtraScaleX);

        for (int i = 0; i < hallBlocks.Length; i++)
        {
            Transform block = hallBlocks[i];

            // Scale
            Vector3 scale = block.localScale;
            scale.x = initialBlockSizeX + extraScaleX;
            block.localScale = scale;

            // Push forward to avoid merging
            float pushDistance = extraScaleX * spacingMultiplier * i;
            Vector3 pos = block.position;
            pos.x = hallBlocks[0].position.x + (initialBlockSizeX * i) + pushDistance;
            block.position = pos;

            // Spiral rotation
            float rotationAmount =
                (playerProgress * rotationSpeed) +
                (i * rotationOffsetPerBlock);

            block.rotation = Quaternion.Euler(0f, 0f, rotationAmount);
        }
    }
}
