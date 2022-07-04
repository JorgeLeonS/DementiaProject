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
/// Every time a player needs to say or do something, an internal counter <see cref="dialogueCounter"/> 
/// is incremented to keep track on that the player is doing.
/// </summary>

//[System.Serializable] public class PlayerCompletedAction : UnityEvent<bool> { }
//[System.Serializable] public class PlayerInteraction : UnityEvent { }

public class PlayerController : MonoBehaviour
{
    public XROrigin MyXROrigin;
    private XRRayInteractor LeftXRInteractor;
    private XRRayInteractor RightXRInteractor;

    public UnityEvent PlayerCompletedInteraction = new UnityEvent();
    public UnityEvent PlayerDialogue = new UnityEvent();
    public UnityEvent PlayerAction = new UnityEvent();
    public event Action InteractWithSomething;

    public List<GameObject> itemsToInteract;

    // TODO Move onto a parent class
    public List<string> DialogueText;
    public List<AudioClip> DialogueAudios;
    public List<float> DialogueDurations;

    // Player subtitles
    private GameObject Canvas;
    //private DialogueAnimator AnimatedText;
    private GameObject TextObject;
    private DialogueAnimator AnimatedTextObject;
    private TextRevealer TRAnimatedTextObject;


    private int dialogueCounter;
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
            // Directly assign LeftXRInteractor and RightXRInteractor components.
            LeftXRInteractor = MyXROrigin.transform.GetChild(0).Find("LeftHandController").GetComponent<XRRayInteractor>();
            RightXRInteractor = MyXROrigin.transform.GetChild(0).Find("RightHandController").GetComponent<XRRayInteractor>();

            // Directly assign Canvas component.
            Canvas = MyXROrigin.transform.Find("FollowingCanvas_DialogueBox_Player").gameObject;
            // Directly assign AnimatedText component First access the image, then the text.
            TextObject = Canvas.transform.GetChild(0).gameObject;
            AnimatedTextObject = TextObject.GetComponent<DialogueAnimator>();
            TRAnimatedTextObject = TextObject.GetComponent<TextRevealer>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"A component of the XROrigin could not be found on the scene! \n" +
                $"Is the naming correct? \n {e}");
        }
    }

    /// <summary>
    /// Add the listerner for performing an actrion and move to laying down postion (In bed).
    /// </summary>
    void Start()
    {
        PlayerDialogue.AddListener(PerformDialogue);
        PlayerAction.AddListener(PerformAction);
        //StartCoroutine(HaveAFirsttext());
        MoveToLayingDownPosition();
    }

    /// <summary>
    /// For testing purposes.
    /// </summary>
    void Update()
    {
    }

    IEnumerator Cor_PerformAction()
    {
        #region Testing purposes only (DELETE WHEN DONE)
        if (interactionCounter == 0)
        {
            InteractionsManager.hasCharacterCorFinished = false;
            yield return StartCoroutine(FadeCanvas.FadeInOutWithAction(MoveToStandingPosition));
            interactionCounter++;
            InteractionsManager.hasCharacterCorFinished = true;
        }
        else if (interactionCounter == 1)
        {
            InteractionsManager.hasCharacterCorFinished = false;
            yield return StartCoroutine(WakeUpScene_Methods.OpenBlinds());
            interactionCounter++;
            InteractionsManager.hasCharacterCorFinished = true;
        }
        else
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("No more player actions!");
        }
        #endregion
    }

    public void PerformAction()
    {
        Debug.Log($"Diana action {interactionCounter}");
        StartCoroutine(Cor_PerformAction());
        PlayerCompletedInteraction.Invoke();
    }

    /// <summary>
    /// Mehtod that is called through the event listener.
    /// It's pretty much only used to call the coroutine of the same name.
    /// </summary>
    public void PerformDialogue()
    {
        Debug.Log($"Diana dialogue {dialogueCounter}");
        StartCoroutine(Cor_PerformDialogue());
        PlayerCompletedInteraction.Invoke();
    }


    /// <summary>
    /// Coroutine that is used to call the next dialogue of the player.
    /// TODO IMPLEMENT WITH ACTION
    /// </summary>
    /// <returns></returns>
    IEnumerator Cor_PerformDialogue()
    {
        if (dialogueCounter >= DialogueText.Count)
        {
            Debug.Log($"Bad action, the player has no more actions!");
        }
        else
        {
            InteractionsManager.hasCharacterCorFinished = false;

            yield return Cor_NextDialogue();
            InteractionsManager.hasCharacterCorFinished = true;
            dialogueCounter++;
        }
    }

    IEnumerator HaveAFirsttext()
    {
        TextObject.SetActive(true);
        AnimatedTextObject.text = "testtext";
        yield return new WaitForSeconds(0.5f);
        TextObject.SetActive(false);
    }

    private void DestroySlicedTextRevealer()
    {
        Transform sliced = Canvas.transform.Find(TextObject.name + "_sliced");
        if (sliced != null)
        {
            GameObject.DestroyImmediate(sliced.gameObject);
        }
    }

    /// <summary>
    /// When a player has a dialogue, it should play the AudioClip of it,
    /// and display the text on the textbox.
    /// </summary>
    IEnumerator Cor_NextDialogue()
    {
        DestroySlicedTextRevealer();

        //Canvas.GetComponent<Canvas>().enabled = true;

        // TODO Add a condition for
        // Could be DialogueDurations or AudioClip
        //var currentClip = audioSource.clip = characterInteraction.DialogueAudios[interactionCounter];
        AnimatedTextObject.text = DialogueText[dialogueCounter];
        TRAnimatedTextObject.RevealTime = DialogueDurations[dialogueCounter] * 0.5f;

        //TextObject.SetActive(true);
        TRAnimatedTextObject.Reveal();
        //AnimatedText.ReadText(DialogueText[interactionCounter], DialogueDurations[interactionCounter]);

        //audioSource.Play();
        yield return new WaitForSeconds(DialogueDurations[dialogueCounter] + 1.0f);

        TRAnimatedTextObject.Unreveal();

        yield return new WaitForSeconds(TRAnimatedTextObject.UnrevealTime + 1.0f);

        //TextObject.SetActive(false);
        //yield return new WaitForSeconds(TRAnimatedTextObject.UnrevealTime * 1.3f);

        //GetComponent<Animator>().SetBool(characterInteraction.AnimationName[interactionCounter], false);
        //Canvas.GetComponent<Canvas>().enabled = false;
    }

    private void MoveToStandingPosition()
    {
        MyXROrigin.transform.position = new Vector3(-0.5f, 0f, 1f);
        MyXROrigin.transform.localRotation = Quaternion.Euler(0, 150, 0);
    }

    private void MoveToLayingDownPosition()
    {
        MyXROrigin.transform.position = new Vector3(-0.15f, 0.2f, -1.0f);
        MyXROrigin.transform.localRotation = Quaternion.Euler(-50, 90, 0);
    }

    #region Player Events Testing section
    //void PlayerAction(bool hasCompletedAction)
    //{
    //    playerCompletion.Invoke(hasCompletedAction);
    //}

    //public void CheckSelectedInteractable(XRRayInteractor obj)
    //{
    //    Debug.Log(obj.)
    //}

    //void DoPlayerAction()
    //{
    //    Debug.Log("Player is doing an action!");

    //    bool hasCompletedAction = true;
    //    PlayerCompletedAction.Invoke();
    //}
    #endregion
}
