using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class InteractableObject : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    bool interactionState = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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

    public void ReturnToOriginalPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
