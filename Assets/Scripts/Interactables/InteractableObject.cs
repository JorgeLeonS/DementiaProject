using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableObject : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    bool interactionState = false;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void MakeInteractable(bool makeInteractable)
    {
        grabInteractable.enabled = makeInteractable;
    }

    public void InteractionStarted(bool start)
    {
        interactionState = grabInteractable.isSelected;
    }

    public bool InteractingWithObject()
    {
        interactionState = grabInteractable.isSelected;
        return interactionState;
    }
}
