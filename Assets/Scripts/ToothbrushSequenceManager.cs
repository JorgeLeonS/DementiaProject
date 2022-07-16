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
    
    [SerializeField] float fadeTime;

    [Header("Toothbrush Properties")]
    [SerializeField] ToothbrushEffect toothbrush;

    [Header("Others")]
    [SerializeField] Canvas fadeCanvas;
    [SerializeField] Transform jamesCharacter;
    [SerializeField] int currentSequence;

    List<Material> toothbrushMats = new List<Material>();
    float timer;
    int indexSequence;
    float toothbrushMatPower = 0f;
    bool endOfthirdSequence = false;

    

    private void Start()
    {
        indexSequence = currentSequence;
        jamesCharacter.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        toothbrush.gameObject.SetActive(false);
        SetSequence();
        foreach (Material material in toothbrush.GetComponent<Renderer>().materials)
        {
            toothbrushMats.Add(material);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
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
        }
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
            textSign.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }  

    /// <summary>
    /// This method indicates what to do on current sequence.
    /// </summary>
    private void SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                // The user gets the prompt to find the toothbrush.
                StartCoroutine(FirstSequence());
                // ToDo Add toothbrush picture.
                break;
            case 1:
                SecondSequence();
                break;
            case 2:
                StartCoroutine(ThirdSequence());
                break;
            case 3:
                // the user gets the prompt to grab the toothbrush.
                // After grabbing the toothbruh, the scene ends.
                // And the user has to grab the toothtbrush
                // Set screen active: Grab the toothbrush
                // Enable toothbrush collider.
                // fade out
                // change to pill scene
            default:
                Debug.Log("Out of range");
                break;
        }
    }

    IEnumerator FirstSequence()
    {
        Debug.Log("First Sequence");
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut(fadeTime);
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
        yield return new WaitForSeconds(fadeTime*2 + 1f);
        textSign.SetActive(true);
    }

    IEnumerator SecondSequence()
    {
        // After asking for help the first time, the toothbrush slowly appears on the screen.
        Debug.Log("Second Sequence");
        fadeCanvas.gameObject.SetActive(false);
        toothbrush.gameObject.SetActive(true);
        toothbrush.StartToothbrushEffect();
        yield return new WaitForSeconds(5f);
    }


    IEnumerator ThirdSequence()
    {
        Debug.Log("third Sequence");
        // James enters the room and goes towards the user to indicate where is the toothbrush.
        jamesCharacter.gameObject.SetActive(true);
        // James appears next to door
        // James walks towards the user
        // James points at toothbrush
        // wait until james finishes pointing at toothbrush to go to new sequence.
        // the variable endOfThirdSequence is called from something
        yield return new WaitUntil(() => endOfthirdSequence);
        NextSequence();
    }

    IEnumerator FourthSequence()
    {
        yield return null;
    }

    //ToDo Find toothbrush manager

}
