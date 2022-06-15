using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInteractable : MonoBehaviour
{
    public event Action OnInteract;

    public void Interact()
    {
        OnInteract?.Invoke();
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
