using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Class used to manage the sequences of "Bathroom Scene"
/// </summary>
public class ToothbrushSequenceManager: MonoBehaviour
{
    [Header("Text")]
    [SerializeField] List<float> timers = new List<float>();
    [SerializeField] List<string> prompts = new List<string>();
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject textSign;
    private TextRevealer textSignTR;

    [SerializeField] float fadeTime;

    [Header("Toothbrush Properties")]
    [SerializeField] ToothbrushEffect toothbrush;

    [Header("Others")]
    [SerializeField] Canvas fadeCanvas;

    public Door door;

    private InteractableObject interactableObject;

    float timer;
    int indexSequence;
    int indexPrompts;
    bool helpRequested = false;

    private void OnEnable()
    {
        //SceneEvents.current.sceneAction += SetSequence;
    }

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

    /// <summary>
    /// This method indicates what to do on current sequence.
    /// </summary>
    public IEnumerator SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                interactableObject = toothbrush.GetComponent<InteractableObject>();
                if (interactableObject == null)
                {
                    Debug.LogError("Missing InteractableObject Component");
                }
                interactableObject.MakeInteractable(false);
                textSignTR = textSign.transform.GetChild(0).GetComponent<TextRevealer>();
                fadeCanvas.gameObject.SetActive(true);
                FadeCanvas.FadeOut(fadeTime);
                timer = timers[indexPrompts];
                helpButton.gameObject.SetActive(false);
                yield return FirstSequence();
                helpButton.SetActive(true);
                break;
            case 1:
                yield return SecondSequence();
                break;
            case 2:
                yield return FirstSequence();
                yield return OpenDoor();
                break;
            case 3:
                yield return FourthSequence();
                break;
            case 4:
                yield return EndScene();
                break;
            default:
                yield return new WaitForSeconds(1f);
                Debug.Log("IndexSequence Out of range");
                break;
        }

        indexSequence++;
        DestroySlicedTextRevealer();

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
        textSignTR.Unreveal(); 
        indexPrompts++;
        yield return new WaitForSeconds(textSignTR.UnrevealTime);
    }

    IEnumerator OpenDoor()
    {
        door.OpenDoorWithNoTransition();
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator SecondSequence()
    {
        //fadeCanvas.gameObject.SetActive(false);
        
        // Wait until user has asked for help
        yield return new WaitUntil(() => helpRequested);
        helpRequested = false;
        helpButton.SetActive(false);
    }

    IEnumerator FourthSequence()
    {
        //Toothbrush slowly appears on the counter
        toothbrush.StartToothbrushEffect();
        yield return new WaitForSeconds(
            toothbrush.timeToTransitionVisibility);

        // Prompt appears to grab the toothbrush.
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();

        // Makes toothbrush Interactable and waits for player to grab the toothbrush.
        interactableObject.MakeInteractable(true);
        yield return new WaitUntil(() => interactableObject.InteractingWithObject());
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
