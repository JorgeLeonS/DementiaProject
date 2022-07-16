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
    //todo rename to toothbrushSequence

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
    [SerializeField] Transform jamesCharacter;
    [SerializeField] int currentSequence;
    
 
    float timer;
    int indexSequence;
    float toothbrushMatPower = 0f;
    bool helpRequested = false;

    private void OnEnable()
    {
        SceneEvents.current.sceneAction += SetSequence;
    }

    private void Start()
    {
        textSignTR = textSign.transform.GetChild(0).GetComponent<TextRevealer>();
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut(fadeTime);
        
        indexSequence = currentSequence;
        timer = timers[indexSequence];
        //jamesCharacter.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        //SetSequence(); // ToDo Subscribe to Scene Events
        
    }

    private void OnDestroy()
    {
        SceneEvents.current.sceneAction -= SetSequence;
    }

    private void Update()
    {
        // this will be invalid after changing script to be event based.
        /*timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (indexSequence < timers.Count - 1)
            {
                textSign.SetActive(false);
                if (indexSequence == 2)
                    return;
                helpButton.gameObject.SetActive(true);
                
            }
        }
        else
        {
            //Debug.Log($"time left: {timer}");
        }*/
    }

    /// <summary>
    /// This method goes to the next sequence after the interaction of helpButton.
    /// It is called from XR Interactable Event or from another method.
    /// </summary>
    public void NextSequence()
    {
        indexSequence++;
        timer = timers[indexSequence];
        SetSequence();
        if (indexSequence < timers.Count)
        {
            helpButton.gameObject.SetActive(false);
            textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
            timer = timers[indexSequence];
            //textSign.SetActive(true);
            textSignTR.Reveal();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }  

    /// <summary>
    /// This method indicates what to do on current sequence.
    /// </summary>
    public IEnumerator SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                // The user gets the prompt to find the toothbrush.
                yield return FirstSequence();
                // ToDo Add toothbrush picture.
                break;
            case 1:
                yield return SecondSequence();
                break;
            case 2:
                yield return FirstSequence();
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
        //helpButton.gameObject.SetActive(false);
        DestroySlicedTextRevealer();
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
        timer = timers[indexSequence];
        //textSign.SetActive(true);
        //return Button to Normal State
        EventSystem.current.SetSelectedGameObject(null);
        
        SceneEvents.current.CompletedInteraction();
    }

    IEnumerator FirstSequence()
    {
        // Small Delay before the start
        //textSign.GetComponentInChildren<TextRevealer>().enabled = false;
        yield return new WaitForSeconds(5f);

        // Show prompt to Find the toothbrush
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
        //textSign.SetActive(true);
        textSignTR.Reveal();
        //textSign.GetComponentInChildren<TextRevealer>().enabled = true;

        // Give time to the user to find the toothbrush
        yield return new WaitForSeconds(10f);
        //textSign.SetActive(false);
        textSignTR.Unreveal();

        // ToDo if()
        helpButton.SetActive(true);
    }

    IEnumerator SecondSequence()
    {
        // After asking for help the first time, the toothbrush slowly appears on the screen.
        Debug.Log("Second Sequence");
        fadeCanvas.gameObject.SetActive(false);
        
        // Wait until user has asked for help
        yield return new WaitUntil(() => helpRequested);
        helpRequested = false;
        helpButton.SetActive(false);
    }

    /*
    IEnumerator ThirdSequence()
    {
        Debug.Log("third Sequence");
        bool endOfSequence = false;
        // James enters the room and goes towards the user to indicate where is the toothbrush.
        jamesCharacter.gameObject.SetActive(true);
        // James appears next to door
        // James walks towards the user
        // James points at toothbrush
        // wait until james finishes pointing at toothbrush to go to new sequence.
        // the variable endOfSequence is called from something
        yield return new WaitUntil(() => endOfSequence);
        //NextSequence();
    }*/

    IEnumerator FourthSequence()
    {
        
        // After grabbing the toothbruh, the scene ends.
        // And the user has to grab the toothtbrush
        // Set screen active: Grab the toothbrush
        toothbrush.StartToothbrushEffect();
        yield return new WaitForSeconds(
            toothbrush.timeToTransitionVisibility);
        
        // ToDo the user gets the prompt to grab the toothbrush.
        textSignTR.Reveal();
        // 
        

        // ToDo wait player to grab toothbrush
        yield return new WaitForSeconds(3f);

        // Enable toothbrush collider.
        // fade out
        // change to pill scene
    }

    IEnumerator EndScene()
    {
        yield return null;
        MenuControl.LoadLevel("MainMenu");
    }

    

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
