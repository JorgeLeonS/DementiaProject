using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : MonoBehaviour
{
    //[SerializeField] Canvas openCanvas;
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
        //openCanvas.gameObject.SetActive(makeInteractable);
        handle.GetComponent<XRSimpleInteractable>().enabled = makeInteractable;
        arrow.gameObject.SetActive(makeInteractable);
    }

    public void OpenDoor()
    {
        // Called from Button or XR Interactable.
        animator.SetBool("Open", true);
        //openCanvas.gameObject.SetActive(false);
        arrow.gameObject.SetActive(false);
        Debug.Log("Start fade out");
        Invoke(nameof(ChangeScene), 1.20f);
    }

    private void ChangeScene()
    {
        if (menuControl != null)
            menuControl.LoadLevel(nextLevel);
        else
            Debug.LogError("Add the MenuControl script to the scene");
    }

    /*public void CloseDoor()
    {
        animator.SetBool("newScene", true);
        animator.SetBool("Open", false);
    }*/


}
