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
    [SerializeField] Button aboutButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button chapterSelectionButton;

    [SerializeField] Canvas aboutCanvas;
    [SerializeField] Canvas tutorialCanvas;
    [SerializeField] Canvas chaptersCanvas;
    [SerializeField] Canvas exitConfirmation;

    List<Canvas> pages = new List<Canvas>();

    [SerializeField] Light defectiveLamp_Light;
    [SerializeField] AudioSource defectiveLamp_Audio;
    [SerializeField] AudioClip bulbPop_AudioClip;

    private void Start()
    {
        pages.Add(aboutCanvas);
        pages.Add(tutorialCanvas);
        pages.Add(chaptersCanvas);
        pages.Add(exitConfirmation);
        HideAllPages();

        Lights_Manager.ChangeAmbientLightIntensity(0.1f, 0.1f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        float duration = defectiveLamp_Audio.clip.length / 2;
        StartCoroutine(Lights_Manager.FadeInAndOutRepeatALight(defectiveLamp_Light, duration));
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartFirstScene);
        aboutButton.onClick.AddListener(ShowAboutScreen);
        tutorialButton.onClick.AddListener(ShowTutorialScreen);
        creditsButton.onClick.AddListener(GoToCredits);
        quitButton.onClick.AddListener(ShowExitConfirmationPage);
        chapterSelectionButton.onClick.AddListener(ShowChapterSelectionScreen);

    }

    private void OnDisable()
    {
        startButton.onClick.RemoveAllListeners();
        aboutButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        creditsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        chapterSelectionButton.onClick.RemoveAllListeners();
    }

    private void StartFirstScene()
    {
        HideAllPages();
        defectiveLamp_Audio.Pause();
        defectiveLamp_Audio.loop = false;
        StopAllCoroutines();
        StartCoroutine(BulbPop());
    }

    IEnumerator BulbPop()
    {
        yield return StartCoroutine(Lights_Manager.FadeInAndOutALight(defectiveLamp_Light, true));
        defectiveLamp_Light.intensity = 0;
        defectiveLamp_Audio.clip = bulbPop_AudioClip;
        defectiveLamp_Audio.Play();
        yield return new WaitForSeconds(0.5f);
        MenuControl.LoadLevel("WakeUpScene2");
    }

    private void ShowChapterSelectionScreen()
    {
        
        ShowSelectedPage(chaptersCanvas);
    }

    private void ShowAboutScreen()
    {

        ShowSelectedPage(aboutCanvas);
    }

    private void ShowTutorialScreen()
    {
        
        ShowSelectedPage(tutorialCanvas);
    }

    private void GoToCredits()
    {
        HideAllPages();
        MenuControl.LoadLevel("CreditsScene");
    }

    public void ExitExperience()
    {
        //Debug.Log("Exit");
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
