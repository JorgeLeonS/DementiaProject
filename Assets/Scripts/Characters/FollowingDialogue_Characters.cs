using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingDialogue_Characters : MonoBehaviour
{
    // Usually the XR Rig, MainCamera on Scene
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // The Dialogue box of the characters will always be looking at the player
        transform.LookAt(target);
    }
}
