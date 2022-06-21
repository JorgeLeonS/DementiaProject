using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class is used to control what the player does.
/// The player can either have a dialogue to say, or an action to perform.
/// It is primarily referenced from the <see cref="InteractionsManager"/> class.
/// Every time a player needs to say or do something, an internal counter <see cref="interactionCounter"/> 
/// is incremented to keep track on that the player is doing.
/// </summary>

//[System.Serializable] public class PlayerCompletedAction : UnityEvent<bool> { }
//[System.Serializable] public class PlayerInteraction : UnityEvent { }

public class PlayerController : MonoBehaviour
{
    public XROrigin MyXROrigin;
    private XRRayInteractor LeftXRRayInteractor;
    private XRRayInteractor RightXRRayInteractor;

    public UnityEvent PlayerCompletedAction = new UnityEvent();
    public UnityEvent PlayerInteraction = new UnityEvent();
    public event Action InteractWithSomething;

    public List<GameObject> itemsToInteract;

    // TODO Move onto a parent class
    public List<string> DialogueText;
    public List<AudioClip> DialogueAudios;
    public List<float> DialogueDurations;

    private GameObject Canvas;
    private DialogueAnimator AnimatedText;

    private int interactionCounter;

    /// <summary>
    /// The XROrigin needs to be referenced from Unity, then, through their name  
    /// the next components, are assigned on the Awake function:
    /// LeftXRRayInteractor, RightXRRayInteractor, Canvas, animatedText
    /// </summary>
    private void Awake()
    {
        try
        {
            // Directly assign LeftXRRayInteractor and RightXRRayInteractor components.
            LeftXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("LeftHandController").GetComponent<XRRayInteractor>();
            RightXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("RightHandController").GetComponent<XRRayInteractor>();

            // Directly assign Canvas component.
            Canvas = MyXROrigin.transform.Find("FollowingCanvas_DialogueBox").gameObject;
            // Directly assign AnimatedText component First access the image, then the text.
            AnimatedText = Canvas.transform.GetChild(0).Find("AnimatedText").GetComponent<DialogueAnimator>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"A component of the XROrigin could not be found on the scene! \n" +
                $"Is the naming correct? \n {e}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerInteraction.AddListener(PerformAction);

        // Go to laying down position
        MyXROrigin.transform.position = new Vector3(0.4f, 0f, -1.07f);
        MyXROrigin.transform.Rotate(new Vector3(0, 0, -70));
    }

    //void PlayerAction(bool hasCompletedAction)
    //{
    //    playerCompletion.Invoke(hasCompletedAction);
    //}

    //public void CheckSelectedInteractable(XRRayInteractor obj)
    //{
    //    Debug.Log(obj.)
    //}

    void DoPlayerAction()
    {
        Debug.Log("Player is doing an action!");

        bool hasCompletedAction = true;
        PlayerCompletedAction.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PerformAction();
        }
    }


    // TODO Move onto a parent class
    IEnumerator Cor_NextDialogue()
    {
        Canvas.GetComponent<Canvas>().enabled = true;

        // TODO Add a condition for
        // Could be DialogueDurations or AudioClip
        //var currentClip = audioSource.clip = characterInteraction.DialogueAudios[interactionCounter];
        AnimatedText.ReadText(DialogueText[interactionCounter], DialogueDurations[interactionCounter]);

        //audioSource.Play();
        yield return new WaitForSeconds(DialogueDurations[interactionCounter] + 1.0f);

        //GetComponent<Animator>().SetBool(characterInteraction.AnimationName[interactionCounter], false);
        Canvas.GetComponent<Canvas>().enabled = false;
    }

    // TODO Move onto a parent class
    IEnumerator Cor_PerformAction()
    {
        if (interactionCounter >= DialogueText.Count)
        {
            Debug.Log($"Bad action, the player has no more actions!");
        }
        else
        {
            // If the character has MoveToNextLocation set to true 
            InteractionsManager.hasCharacterCorFinished = false;

            yield return Cor_NextDialogue();
            InteractionsManager.hasCharacterCorFinished = true;
            interactionCounter++;
        }
    }

    private void MovePlayerToStandingPosition()
    {
        MyXROrigin.transform.position = new Vector3(-0.5f, 0f, 1f);
        MyXROrigin.transform.Rotate(new Vector3(0, 0, 70));
    } 

    public void PerformAction()
    {
        Debug.Log($"Diana action {interactionCounter}");
        if (interactionCounter == 2)
        {
            FadeCanvas.FadeInOutWithAction(MovePlayerToStandingPosition);
            PlayerCompletedAction.Invoke();
            interactionCounter++;
        }
        else if (interactionCounter == 3)
        {
            AmbientManager.OpenBlinds();
            PlayerCompletedAction.Invoke();
            interactionCounter++;
        }
        else
        {
            StartCoroutine(Cor_PerformAction());
            PlayerCompletedAction.Invoke();
        }
        
    }
}
