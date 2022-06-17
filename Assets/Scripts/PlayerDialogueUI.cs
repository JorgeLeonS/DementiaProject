using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that makes the UI Subtitles textbox for the player to always follow them.
public class PlayerDialogueUI : MonoBehaviour
{
    public Transform target;
    public float CameraZDistance = 3.0F;
    public float CameraYDistance = -1.0F;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Define my target position in front of the camera ->
        Vector3 targetPosition = target.TransformPoint(new Vector3(0, CameraYDistance, CameraZDistance));

        // Smoothly move my object towards that position ->
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // version 1: my object's rotation is always facing to camera with no dampening  ->
        transform.LookAt(transform.position + target.rotation * Vector3.forward, target.rotation * Vector3.up);

        // version 2 : my object's rotation isn't finished synchronously with the position smooth.damp ->
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, 35 * Time.deltaTime);
    }
}
