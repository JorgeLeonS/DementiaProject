using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to control the behaviour of the Main menu
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button startButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button chapterSelectionButton;

    [SerializeField] Canvas tutorialCanvas;
    [SerializeField] Canvas chaptersCanvas;
    [SerializeField] Canvas exitConfirmation;

    List<Canvas> pages = new List<Canvas>();

    private void Start()
    {
        pages.Add(tutorialCanvas);
        pages.Add(chaptersCanvas);
        pages.Add(exitConfirmation);
        HideAllPages();
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartFirstScene);
        tutorialButton.onClick.AddListener(ShowTutorialScreen);
        creditsButton.onClick.AddListener(GoToCredits);
        quitButton.onClick.AddListener(ShowExitConfirmationPage);
        chapterSelectionButton.onClick.AddListener(ShowChapterSelectionScreen);

    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        chapterSelectionButton.onClick.RemoveAllListeners();
    }

    private void StartFirstScene()
    {
        Debug.Log("Start");
        HideAllPages();
        MenuControl.LoadLevel("WakeUpScene2");
    }

    private void ShowChapterSelectionScreen()
    {
        Debug.Log("Chapter Selection");
        ShowSelectedPage(chaptersCanvas);
    }

    private void ShowTutorialScreen()
    {
        Debug.Log("Tutorial Screen");
        ShowSelectedPage(tutorialCanvas);
    }

    private void GoToCredits()
    {
        HideAllPages();
        Debug.Log("Credits");
    }

    public void ExitExperience()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void ShowExitConfirmationPage()
    {
        ShowSelectedPage(exitConfirmation);
    }

    private void ShowSelectedPage(Canvas selectedCanvas)
    {
        foreach(Canvas page in pages)
        {
            if(page == selectedCanvas)
            {
                page.gameObject.SetActive(true);
                continue;
            }
            page.gameObject.SetActive(false);
        }
    }

    public void HideAllPages()
    {
        foreach(Canvas page in pages)
        {
            page.gameObject.SetActive(false);
        }
    }
}
