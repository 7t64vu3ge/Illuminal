using UnityEngine;

public class HallRotationManager : MonoBehaviour
{
    public GameObject TriggerPoint, Hall1, Hall2, Hall3, Hall4, Hall5, Hall6;
    public float maxDistance = 10f, Hall1_angle = 15f, Hall2_angle = 30f, Hall3_angle = 40f;
    public float Hall4_angle = 50f, Hall5_angle = 60f, Hall6_angle = 70f;
    private TriggerRotation rotationScript;
    private Vector3 d_vector ; // vector3 value of difference between player and collider, starts calculation after colliision
    private Transform Hall1_Transform, Hall2_Transform, Hall3_Transform, Hall4_Transform, Hall5_Transform, Hall6_Transform;
    void Start()
    {
        rotationScript = TriggerPoint.GetComponent<TriggerRotation>();
        Hall1_Transform = Hall1.GetComponent<Transform>();
        Hall2_Transform = Hall2.GetComponent<Transform>();
        Hall3_Transform = Hall3.GetComponent<Transform>();
        Hall4_Transform = Hall4.GetComponent<Transform>();
        Hall5_Transform = Hall5.GetComponent<Transform>();
        Hall6_Transform = Hall6.GetComponent<Transform>();
    }
    void Update()
    {
        d_vector = rotationScript.posDifference;

        float t = Mathf.Clamp(d_vector.z/maxDistance, -1f, 1f);
        float angle1 = t * Hall1_angle;
        float angle2 = t * Hall2_angle;
        float angle3 = t * Hall3_angle;
        float angle4 = t * Hall4_angle;
        float angle5 = t * Hall5_angle;
        float angle6 = t * Hall6_angle;

        Hall1_Transform.localRotation = Quaternion.Euler(angle1,0f,0f);
        Hall2_Transform.localRotation = Quaternion.Euler(angle2,0f,0f);
        Hall3_Transform.localRotation = Quaternion.Euler(angle3,0f,0f);
        Hall4_Transform.localRotation = Quaternion.Euler(angle4,0f,0f);
        Hall5_Transform.localRotation = Quaternion.Euler(angle5,0f,0f);
        Hall6_Transform.localRotation = Quaternion.Euler(angle6,0f,0f);
         

        
    }
}
