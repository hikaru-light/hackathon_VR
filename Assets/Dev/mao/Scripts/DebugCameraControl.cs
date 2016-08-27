using UnityEngine;
using System.Collections;

public class DebugCameraControl : MonoBehaviour
{
#if UNITY_EDITOR
    private float horizontal, vertical;

    Quaternion rotation;

    void Start()
    {
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        horizontal += Input.GetAxis("Horizontal");
        vertical += Input.GetAxis("Vertical");

        var rot = rotation;
        rot *= Quaternion.AngleAxis(horizontal, Vector3.up);
        rot *= Quaternion.AngleAxis(vertical, Vector3.right);
        transform.rotation = rot;
    }
#endif
}