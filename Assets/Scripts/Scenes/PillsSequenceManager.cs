using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PillsSequenceManager : MonoBehaviour
{
    [SerializeField] GameObject textSign;
    private TextRevealer textSignTR;

    [SerializeField] List<string> prompts = new List<string>();

    [SerializeField] float fadeTime;
    [Header("Others")]
    [SerializeField] Canvas fadeCanvas;

    public Door door;
    public DistortionEffect[] pills;

    private List<string> grabbedPills;
    public List<string> pillsToGrab = new List<string>
    {
        "BottlePillNight", "BottlePillAfternoon", "BottlePillMorning"
    };

    int indexSequence;
    int indexPrompts;

    private void Start()
    {
        SceneEvents.current.sceneAction += SetSequence;

        indexSequence = 0;
        indexPrompts = 0;
        
        // TODO Start with the door open instead of opening it here.
        //door.OpenDoorWithNoTransition();

        textSignTR = textSign.transform.GetChild(0).GetComponent<TextRevealer>();
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut(fadeTime);

        grabbedPills = new List<string>();
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
                yield return GrabPillsSequence();
                break;
            case 1:
                yield return GrabPillsSequence();
                break;
            case 2:
                RemamingPill();
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

    /// <summary>
    /// Display "Grab the pills" text,
    /// Enable the colliders for all the pills bottles
    /// Wait for the player to grab one bottle
    /// Disable the colliders
    /// </summary>
    IEnumerator GrabPillsSequence()
    {
        //indexPrompts = 0;
        // Show prompt to Find the toothbrush
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        indexPrompts++;
        // Enable the colliders for all the pills
        foreach (var pill in pills)
        {
            pill.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        string grabbedPill = "";
        yield return new WaitUntil(() => WaitForInteraction(out grabbedPill));
        textSignTR.Unreveal();
        yield return new WaitForSeconds(textSignTR.UnrevealTime);
        grabbedPills.Add(grabbedPill);
        pillsToGrab.Remove(grabbedPill);

        DestroySlicedTextRevealer();
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        foreach (var pill in pills)
        {
            pill.gameObject.GetComponent<BoxCollider>().enabled = false;
        }

    }

    void RemamingPill()
    {
        print(pillsToGrab[0]);
    }

    bool WaitForInteraction(out string pillName)
    {
        bool interaction = false;
        foreach (var pill in pills)
        {
            //pill.GetComponent<InteractableObject>();
            if (pill.GetComponent<InteractableObject>().InteractingWithObject())
            {
                pillName = pill.gameObject.name;
                return interaction = true;
            }
        }
        pillName = "aa";
        return interaction;
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
