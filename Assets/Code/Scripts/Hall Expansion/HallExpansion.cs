using UnityEngine;
using System.Collections.Generic;

public class HallExpansion : MonoBehaviour
{
    public Transform TriggerPoint, ExpandBlock, TranslateBlock;
    public float maxDistance = 20f;
    public float maxExtension = 4f;
    public float offsetx;

    private TriggerRotation rotationScript;
    private Vector3 d_vector, initialScale, translate_block_initial_pos;

    private List<Renderer> renderers = new List<Renderer>();
    private List<Vector2> initialTilings = new List<Vector2>();

    void Start()
    {
        rotationScript = TriggerPoint.GetComponent<TriggerRotation>();
        initialScale = ExpandBlock.localScale;
        translate_block_initial_pos = TranslateBlock.position;

        // 🔥 Grab ALL renderers under IllusionHalls
        Renderer[] found = ExpandBlock.GetComponentsInChildren<Renderer>();

        foreach (var r in found)
        {
            if (r.material != null)
            {
                renderers.Add(r);
                initialTilings.Add(r.material.mainTextureScale);
            }
        }
    }

    void Update()
    {
        d_vector = rotationScript.posDifference;
        float t = Mathf.Clamp(d_vector.z / maxDistance, -1f, 0f);
        float newZ = initialScale.z - t * maxExtension;
        // Scale hall
        ExpandBlock.localScale = new Vector3(
            newZ,
            initialScale.x,
            initialScale.y
        );

        // Translate block
        float delta = (newZ - initialScale.z) * 0.5f;
        TranslateBlock.position = new Vector3(
            translate_block_initial_pos.x ,
            translate_block_initial_pos.y,
            translate_block_initial_pos.z - delta * offsetx
        );

        // 🔥 Fix tiling for all child cubes
        float scaleRatio = newZ/ initialScale.z;

        for (int i = 0; i < renderers.Count; i++)
        {
            if (renderers[i] == null) continue;

            Vector2 baseTiling = initialTilings[i];
            renderers[i].material.mainTextureScale =
                new Vector2(baseTiling.x * scaleRatio, baseTiling.y);
        }
    }
}
