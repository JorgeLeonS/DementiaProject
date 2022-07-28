using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSpawn : MonoBehaviour
{
    [SerializeField] InputActionAsset playerControls;
    [SerializeField] GameObject handMenu;
    bool showMenu = false;
    InputAction displayMenu;

    // Start is called before the first frame update
    void Start()
    {
        var gameplayActionMap = playerControls.FindActionMap("XRI LeftHand");
        displayMenu = gameplayActionMap.FindAction("ShowLittleMenu");

        displayMenu.performed += OnMenuBtnPressed;
        displayMenu.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        handMenu.SetActive(showMenu);
    }

    public void OnMenuBtnPressed(InputAction.CallbackContext context)
    {
        showMenu = !showMenu;
        if (showMenu)
        {
            Resume();
        }
        else
            Pause();
    }

    public void Resume()
    {
        Time.timeScale = 0;
    }

    public void Pause()
    {
        Time.timeScale = 1;
    }
}