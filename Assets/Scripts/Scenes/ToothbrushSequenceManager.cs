using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Class used to manage the sequences of "Toothbrush Scene"
/// </summary>
public class ToothbrushSequenceManager: MonoBehaviour
{
    [Header("Text")]
    //[SerializeField] List<float> timers = new List<float>();
    [SerializeField] List<string> prompts = new List<string>();
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject textSign;
    private TextRevealer textSignTR;

    [SerializeField] float fadeTime;

    [Header("Toothbrush Properties")]
    [SerializeField] ToothbrushEffect toothbrush;

    [Header("Others")]
    [SerializeField] Canvas fadeCanvas;

    [SerializeField]
    private AudioSource doorAudio;
    public Door door;

    private InteractableObject interactableObject;

    //float timer;
    int indexSequence;
    int indexPrompts;
    bool helpRequested = false;

    private void Start()
    {
        indexPrompts = 0;
        indexSequence = 0;
        helpButton.gameObject.SetActive(false);
        SceneEvents.current.sceneAction += SetSequence;
    }

    private void OnDestroy()
    {
        SceneEvents.current.sceneAction -= SetSequence;
    }

    private void LoadObjects()
    {
        interactableObject = toothbrush.GetComponent<InteractableObject>();
        if (interactableObject == null)
        {
            Debug.LogError("Missing InteractableObject Component");
        }
        interactableObject.MakeInteractable(false);
        textSignTR = textSign.transform.GetChild(0).GetComponent<TextRevealer>();
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut(fadeTime);
        //timer = timers[indexPrompts];
        helpButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// This method indicates what to do on current sequence.
    /// </summary>
    public IEnumerator SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                LoadObjects();
                yield return FirstSequence();
                helpButton.SetActive(true);
                break;
            case 1:
                yield return SecondSequence();
                break;
            case 2:
                yield return ThirdSequence();
                yield return OpenDoor();
                break;
            case 3:
                yield return ToothbrushAppears();
                break;
            case 4:
                yield return FourthSequence();
                break;
            case 5:
                yield return EndScene();
                break;
            default:
                yield return new WaitForSeconds(1f);
                Debug.Log("IndexSequence Out of range");
                break;
        }

        indexSequence++;

        // Returns Button to Normal State.
        EventSystem.current.SetSelectedGameObject(null);
        
        //SceneEvents.current.CompletedInteraction();
    }

    IEnumerator FirstSequence()
    {
        // Show prompt to Find the toothbrush
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        // Give time to the user to find the toothbrush
        yield return new WaitForSeconds(10f);
    }

    IEnumerator ThirdSequence()
    {
        // Show prompt to Find the toothbrush
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        // Give time to the user to find the toothbrush
        yield return new WaitForSeconds(10f);

        textSignTR.Unreveal();
        DestroySlicedTextRevealer();

        indexPrompts++;
    }

    IEnumerator OpenDoor()
    {
        door.OpenDoorWithNoTransition();
        doorAudio.Play();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator SecondSequence()
    {
        // Wait until user has asked for help
        yield return new WaitUntil(() => helpRequested);

        // Unreveal past text
        textSignTR.Unreveal();
        DestroySlicedTextRevealer();

        indexPrompts++;
        yield return new WaitForSeconds(textSignTR.UnrevealTime);

        helpRequested = false;
        helpButton.SetActive(false);
    }

    IEnumerator ToothbrushAppears()
    {
        //Toothbrush slowly appears on the counter
        toothbrush.StartToothbrushEffect();
        yield return new WaitForSeconds(
            toothbrush.timeToTransitionVisibility);
    }

    IEnumerator FourthSequence()
    {
        // Prompt appears to grab the toothbrush.
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();

        // Makes toothbrush Interactable and waits for player to grab the toothbrush.
        interactableObject.MakeInteractable(true);
        yield return new WaitUntil(() => interactableObject.InteractingWithObject());

        // Unreveal past text
        textSignTR.Unreveal();
        DestroySlicedTextRevealer();
    }

    IEnumerator EndScene()
    {
        yield return null;
        MenuControl.LoadLevel("PillsScene");
    }

    // Called from OnClick event
    public void RequestHelp()
    {
        helpRequested = true;
    }

    private void DestroySlicedTextRevealer()
    {
        Transform sliced = textSign.transform.Find(textSign.transform.GetChild(0).name + "_sliced");
        if (sliced != null)
        {
            GameObject.DestroyImmediate(sliced.gameObject);
        }
    }

}
