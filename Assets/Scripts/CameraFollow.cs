using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objToFollow;
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = objToFollow.position + offset;
        transform.LookAt(objToFollow);
    }
}
