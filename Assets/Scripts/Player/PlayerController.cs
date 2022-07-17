using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class is used to control what the player does.
/// The player can either have a dialogue to say, or an action to perform.
/// It is primarily referenced from the <see cref="InteractionsManager"/> class.
/// Every time a player needs to say or do something, an internal counter <see cref="dialogueCounter"/> 
/// is incremented to keep track on that the player is doing.
/// </summary>

public class PlayerController : MonoBehaviour
{
    public XROrigin MyXROrigin;
    private XRRayInteractor LeftXRInteractor;
    private XRRayInteractor RightXRInteractor;

    //public event Action InteractWithSomething;

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
    /// Add the listerner for performing an action and move to laying down postion (In bed).
    /// When the script subscribes to the playerAction Event, the Coroutine is managed by a switch that depends on the counter.
    /// It's very important that the Scene name on the switch matches exactly.
    /// </summary>
    void Start()
    {
        SceneEvents.current.playerDialogue += Cor_PerformDialogue;
        switch (SceneManager.GetActiveScene().name)
        {
            case "WakeUpScene":
                SceneEvents.current.playerAction += Cor_PerformAction_WakeUpScene;
                MoveToLayingDownPosition();
                break;
            case "ToothbrushScene":
                // toDo something

                break;
            default:
                Debug.LogWarning("The scene name might not match!");
                break;
        }
        //SceneEvents.current.playerAction += Cor_PerformAction;
        //StartCoroutine(HaveAFirsttext());
        
        
    }

    private void OnDestroy()
    {
        SceneEvents.current.playerDialogue -= Cor_PerformDialogue;
        switch (SceneManager.GetActiveScene().name)
        {
            case "WakeUpScene":
                SceneEvents.current.playerAction -= Cor_PerformAction_WakeUpScene;
                break;
            case "ToothbrushScene":
                // toDo something
                break;
            default:
                Debug.LogWarning("The scene name might not match!");
                break;
        }
    }

    /// <summary>
    /// Coroutine that is used to call the next dialogue of the player.
    /// </summary>
    IEnumerator Cor_PerformDialogue()
    {
        if (dialogueCounter >= DialogueText.Count)
        {
            yield return new WaitForSeconds(3f);
            Debug.LogWarning($"Bad action, the player has no more dialogues!");
            //SceneEvents.current.CompletedAction();
        }
        else
        {
            yield return Cor_NextDialogue();
            dialogueCounter++;
            //SceneEvents.current.CompletedAction();
        }
    }

    /// <summary>
    /// When a player has a dialogue, it will display the subtitle text with the TextRevealerPro asset.
    /// </summary>
    IEnumerator Cor_NextDialogue()
    {
        DestroySlicedTextRevealer();

        AnimatedTextObject.text = DialogueText[dialogueCounter];

        TRAnimatedTextObject.RevealTime = DialogueDurations[dialogueCounter] * 0.5f;

        TRAnimatedTextObject.Reveal();

        yield return new WaitForSeconds(DialogueDurations[dialogueCounter] + 1.0f);

        TRAnimatedTextObject.Unreveal();

        yield return new WaitForSeconds(TRAnimatedTextObject.UnrevealTime + 1.0f);
    }

    /// <summary>
    /// Method used for th TextRevealerPro asset, 
    /// needed to destroy the created 'slices' every time the text needs to change.
    /// </summary>
    private void DestroySlicedTextRevealer()
    {
        Transform sliced = Canvas.transform.Find(TextObject.name + "_sliced");
        if (sliced != null)
        {
            GameObject.DestroyImmediate(sliced.gameObject);
        }
    }

    #region Actions section

    /// <summary>
    /// Switch based method that is used to call the different actions the player needs to do on the Scene: WakeUpScene
    /// </summary>
    IEnumerator Cor_PerformAction_WakeUpScene()
    {
        switch (interactionCounter)
        {
            case 0:
                yield return StartCoroutine(FadeCanvas.FadeInOutWithAction(MoveToStandingPosition));
                break;
            default:
                yield return new WaitForSeconds(3f);
                Debug.LogWarning("No more player actions!");
                break;
        }
        interactionCounter++;
        //SceneEvents.current.CompletedAction();
    }
    #endregion

    #region Other Player's methods
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
    #endregion

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
