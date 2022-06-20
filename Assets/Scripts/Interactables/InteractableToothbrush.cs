using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableToothbrush : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<PlayerController>().InteractWithSomething += TriggerToothbrush_OnInteract;
    }
    public void TriggerToothbrush_OnInteract()
    {
        Debug.Log("Toooth");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
