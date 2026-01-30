using UnityEngine;

public class SitPoint : MonoBehaviour
{
    public Transform sitPosition;
    public float sitHeightOffset = 0f;

    public Vector3 GetSitWorldPosition()
    {
        if (sitPosition != null)
            return sitPosition.position + Vector3.up * sitHeightOffset;

        return transform.position + Vector3.up * sitHeightOffset;
    }

    public Vector3 GetForward()
    {
        if (sitPosition != null)
            return sitPosition.forward;

        return transform.forward;
    }
}
