using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : MonoBehaviour
{
    [SerializeField] string nextLevel;
    [SerializeField] Transform handle;
    [SerializeField] Transform arrow;
    Animator animator;

    [SerializeField] bool isTest = false;

    private void Start()
    {
        StartInteraction(isTest);
        animator = GetComponent<Animator>();
    }

    public void StartInteraction(bool makeInteractable)
    {
        handle.GetComponent<XRSimpleInteractable>().enabled = makeInteractable;
        arrow.gameObject.SetActive(makeInteractable);
    }

    public void OpenDoor()
    {
        // Called from Button or XR Interactable.
        animator.SetBool("Open", true);
        arrow.gameObject.SetActive(false);
        FadeCanvas.FadeIn().OnComplete(() => Invoke(nameof(ChangeScene), 1.20f));
    }

    public void OpenDoorWithNoTransition()
    {
        // Called from Button or XR Interactable.
        animator.SetBool("Open", true);
        arrow.gameObject.SetActive(false);
    }

    private void ChangeScene()
    {
        MenuControl.LoadLevel(nextLevel);
    }
}
