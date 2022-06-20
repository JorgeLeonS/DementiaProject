using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Canvas openCanvas;
    [SerializeField] string nextLevel;
    Animator animator;
    MenuControl menuControl;

    private void Start()
    {
        StartInteraction(true);
        animator = GetComponent<Animator>();
        menuControl = FindObjectOfType<MenuControl>();
    }

    public void StartInteraction(bool makeInteractable)
    {
        openCanvas.gameObject.SetActive(makeInteractable);
    }

    public void OpenDoor()
    {
        // Called from Button.
        animator.SetBool("Open", true);
        openCanvas.gameObject.SetActive(false);
        Debug.Log("Start fade out");
        Invoke(nameof(ChangeScene), 1.02f);
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
