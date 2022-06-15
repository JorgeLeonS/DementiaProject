using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFridge : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<CustomInteractable>().OnInteract += TriggerFridge_OnInteract;
    }

    public void TriggerFridge_OnInteract()
    {
        Debug.Log("Fridgeeee");
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
