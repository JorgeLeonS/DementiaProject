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
    MenuControl menuControl;

    [SerializeField] bool isTest = false;

    private void Start()
    {
        StartInteraction(isTest);
        animator = GetComponent<Animator>();
        menuControl = FindObjectOfType<MenuControl>();
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

    private void ChangeScene()
    {
        if (menuControl != null)
            menuControl.LoadLevel(nextLevel);
        else
            Debug.LogError("Add the MenuControl script to the scene");
    }
}
