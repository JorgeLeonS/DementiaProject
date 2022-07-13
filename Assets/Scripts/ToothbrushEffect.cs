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
public class ToothbrushEffect : MonoBehaviour
{
    //todo rename to toothbrushSequence

    [SerializeField] List<float> timers = new List<float>();
    [SerializeField] List<string> prompts = new List<string>();
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject textSign;
    [SerializeField] Transform toothbrush;
    //[SerializeField] float fadeTime = 1f;
    [Range(0f,5f)]
    [SerializeField] float toothbrushVisibility = 1f;
    [SerializeField] Canvas fadeCanvas;

    List<Material> toothbrushMats = new List<Material>();
    float timer;
    int indexSequence = 0;
    float toothbrushMatPower = 0f;
    bool endOfthirdSequence = false;

    private void OnEnable()
    {
        helpButton.GetComponent<Button>().onClick.AddListener(NextSequence);
    }

    private void OnDisable()
    {
        helpButton.GetComponent<Button>().onClick.RemoveListener(NextSequence);
    }

    private void Start()
    {
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut();
        timer = timers[indexSequence];
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
            Debug.Log($"time left: {timer}");
        }

        /*
        // Second Sequence. It changes the toothbrush visibiity over an amount of time
        if(indexSequence == 1)
        {
            toothbrushtimer += Time.deltaTime;
            if(toothbrushtimer < 5f)
            {
                foreach (Material material in toothbrushMats)
                {
                    material.SetFloat("_Power", toothbrushtimer * 0.1f);
                }
            }
        }*/
    }

    /// <summary>
    /// This method goes to the next sequence after the interaction of helpButton.
    /// </summary>
    private void NextSequence()
    {
        indexSequence++;
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
                helpButton.gameObject.SetActive(false);
                toothbrush.gameObject.SetActive(false);
                textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
                textSign.SetActive(true);
                // ToDo Add toothbrush picture.
                break;
            case 1:
                StartCoroutine(SecondSequence());
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

    IEnumerator SecondSequence()
    {
        // After asking for help the first time, the toothbrush slowly appears on the screen.
        fadeCanvas.gameObject.SetActive(false);
        toothbrush.gameObject.SetActive(true);
        MakeToothbrushVisible(3f);
        yield return null;
    }

    private Tween MakeToothbrushVisible(float duration)
    {
        
        return DOVirtual.Float(toothbrushMatPower, toothbrushVisibility, duration, newVal => {
            toothbrushMatPower = newVal;
            toothbrush.GetComponent<Renderer>().material.SetFloat("_Power", toothbrushMatPower);
        });
    }

    IEnumerator ThirdSequence()
    {
        // James enters the room and goes towards the user to indicate where is the toothbrush.

        // James appears next to door
        // James walks towards the user
        // James points at toothbrush

        // wait until james finishes pointing at toothbrush to go to new sequence.
        // the variable endOfThirdSequence is called from Animation timeline event
        yield return new WaitUntil(() => endOfthirdSequence);
        NextSequence();
    }

}
