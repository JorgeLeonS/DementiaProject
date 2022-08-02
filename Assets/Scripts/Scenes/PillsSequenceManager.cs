using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class PillsSequenceManager : MonoBehaviour
{
    public CharacterController James;

    public Volume sceneVolume;
    private ChromaticAberration chA;
    private LensDistortion lD;
    private FilmGrain fG;

    bool triggerEffects = false;

    XRIDefaultInputActions inputControllers;
    bool isAGripBeingPressed = false;

    [SerializeField] GameObject textSign;
    private TextRevealer textSignTR;

    [SerializeField] List<string> prompts = new List<string>();

    [SerializeField] float fadeTime;
    [Header("Others")]
    [SerializeField] Canvas fadeCanvas;

    public Door door;
    public List<DistortionEffect> pills;

    InteractableObject grabbedPill;
    private string grabbedPillName;
    private bool aPillHasBeenGrabbed;
    //private List<Vector3> pillsPositions;
    private Dictionary<string, Vector3> pillsPositions;
    public List<string> pillsToGrab = new List<string>

    {
        "BottlePillNight", "BottlePillAfternoon", "BottlePillMorning"
    };


    int indexSequence;
    int indexPrompts;

    void Start()
    {
        // Subscribe to interactions manager event
        SceneEvents.current.sceneAction += SetSequence;

        indexSequence = 0;
        indexPrompts = 0;

        textSignTR = textSign.transform.GetChild(0).GetComponent<TextRevealer>();
        fadeCanvas.gameObject.SetActive(true);
        FadeCanvas.FadeOut(fadeTime);

        pillsPositions = new Dictionary<string, Vector3>();

        foreach (var pill in pills)
        {
            pillsPositions.Add(pill.gameObject.name, pill.transform.localPosition);
        }

        try
        {
            if (sceneVolume == null)
            {
                sceneVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"PostProcessVolume could not be found on the scene! \n" +
                $"Is the naming correct?\n {e}");
        }

        sceneVolume.profile.TryGet<ChromaticAberration>(out chA);
        sceneVolume.profile.TryGet<LensDistortion>(out lD);
        sceneVolume.profile.TryGet<FilmGrain>(out fG);

    }

    private void Awake()
    {
        inputControllers = new XRIDefaultInputActions();

        inputControllers.XRILeftHand.Enable();
        inputControllers.XRILeftHand.Select.canceled += cntxt => isAGripBeingPressed = false;
        inputControllers.XRILeftHand.Select.performed += cntxt => isAGripBeingPressed = true;

        inputControllers.XRIRightHand.Enable();
        inputControllers.XRIRightHand.Select.canceled += cntxt => isAGripBeingPressed = false;
        inputControllers.XRIRightHand.Select.performed += cntxt => isAGripBeingPressed = true;
    }

    private void Update()
    {
        chA.intensity.value = Mathf.Lerp(0.1f, 1f, Mathf.PingPong(Time.time, 1));
        lD.intensity.value = Mathf.Lerp(0f, 0.5f, Mathf.PingPong(Time.time/2, 1));

        //if (Input.GetKeyDown(KeyCode.Space))
        //    triggerEffects = !triggerEffects;

        if (triggerEffects)
        {
            chA.active = true;
            lD.active = true;
            fG.active = true;
        }
        else
        {
            chA.active = false;
            lD.active = false;
            fG.active = false;
        }
    }

    private void OnDestroy()
    {
        SceneEvents.current.sceneAction -= SetSequence;
        inputControllers.XRILeftHand.Disable();
        inputControllers.XRIRightHand.Disable();
    }

    /// <summary>
    /// This method indicates what to do on current sequence.
    /// </summary>
    public IEnumerator SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                // TODO Start with the door open instead of opening it here.
                door.OpenDoorWithNoTransition();
                yield return GrabFirstPillsSequence();
                break;
            case 1:
                yield return GrabSecondPillsSequence();
                break;
            case 2:
                yield return RemainingPill();
                break;
            case 3:
                yield return EndScene();
                break;
            default:
                yield return new WaitForSeconds(1f);
                Debug.LogWarning("IndexSequence Out of range");
                break;
        }

        indexSequence++;
        //DestroySlicedTextRevealer();

        // Returns Button to Normal State.
        //EventSystem.current.SetSelectedGameObject(null);

        //SceneEvents.current.CompletedInteraction();
    }

    /// <summary>
    /// Display "Grab the pills" text,
    /// Enable the colliders for all the pills bottles
    /// Wait for the player to grab one bottle
    /// Disable the colliders
    /// </summary>
    IEnumerator GrabFirstPillsSequence()
    {
        // Show prompt to grab the pills
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        indexPrompts++;
        yield return new WaitForSeconds(textSignTR.RevealTime);

        EnableInteractionForPills();

        // Wait for the user to grab one of the pills
        grabbedPill = null;
        yield return WaitForUserToGrabAPill();

        // When a pill bottle has been grabbed, prompt to take another one
        //textSignTR.Unreveal();
        //yield return new WaitForSeconds(textSignTR.UnrevealTime);
        DestroySlicedTextRevealer();
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        textSignTR.Reveal();
        indexPrompts++;

        yield return WaitForUserToLetGoOfPill();
    }

    IEnumerator GrabSecondPillsSequence()
    {
        EnableInteractionForPills();

        // Wait for the user to grab one of the pills
        grabbedPill = null;
        yield return WaitForUserToGrabAPill();

        // When a pill bottle has been grabbed, prompt to take another one
        //textSignTR.Unreveal();
        //yield return new WaitForSeconds(textSignTR.UnrevealTime);
        DestroySlicedTextRevealer();
        //textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexPrompts];
        //textSignTR.Reveal();
        //indexPrompts++;

        yield return WaitForUserToLetGoOfPill();
    }

    IEnumerator RemainingPill()
    {
        string toBeSearched = "BottlePill";
        int index = pillsToGrab[0].IndexOf(toBeSearched);
        string day = pillsToGrab[0].Substring(index + toBeSearched.Length);
        string color = "";

        switch (day)
        {
            case "Monday":
                color = "Green";
                break;
            case "Wednesday":
                color = "Orange";
                break;
            case "Friday":
                color = "Blue";
                break;
        }

        yield return James.Cor_CustomDialogue($"Today is {day}, so take the ones on the {color} bottle.", 3);
    }

    IEnumerator EndScene()
    {
        yield return null;
        MenuControl.LoadLevel("MainMenu2");
    }

    void EnableInteractionForPills()
    {
        // Enable the colliders for the rest of the pills
        foreach (var pill in pills)
        {
            pill.gameObject.GetComponent<BoxCollider>().enabled = true;
            pill.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
        }
    }

    IEnumerator WaitForUserToGrabAPill()
    {
        aPillHasBeenGrabbed = false;

        while (!aPillHasBeenGrabbed)
        {
            yield return new WaitUntil(() => isAGripBeingPressed);

            foreach (var pill in pills)
            {
                if (pill.GetComponent<InteractableObject>().InteractingWithObject())
                {
                    grabbedPillName = pill.gameObject.name;
                    aPillHasBeenGrabbed = true;
                }
            }
        }
        triggerEffects = true;
        yield return new WaitUntil(() => aPillHasBeenGrabbed);

    }

    IEnumerator WaitForUserToLetGoOfPill()
    {
        // Wait the user has let go of the trigger
        yield return new WaitWhile(() => isAGripBeingPressed);
        triggerEffects = false;
        // Take those pills out of the list of strings
        pillsToGrab.Remove(grabbedPillName);

        // Get the grabbed pill and return it to its original position
        grabbedPill = GameObject.Find(grabbedPillName).GetComponent<InteractableObject>();
        grabbedPill.transform.localPosition = pillsPositions[grabbedPill.gameObject.name];
        grabbedPill.transform.localRotation = new Quaternion(0, 0, 0, 0);

        // Disable the colliders on all the pills
        foreach (var pill in pills)
        {
            pill.gameObject.GetComponent<BoxCollider>().enabled = false;
            pill.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        }

        pills.Remove(grabbedPill.GetComponent<DistortionEffect>());

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
