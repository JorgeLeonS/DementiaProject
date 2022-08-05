using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSpectatorCamera : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 0.5f;
    public float rotationSpeed = 6;

    private Vector3 velocitySpeed = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocitySpeed, smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * rotationSpeed);
    }
}
