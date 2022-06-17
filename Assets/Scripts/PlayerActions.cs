using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//[System.Serializable] public class PlayerCompletedAction : UnityEvent<bool> { }
//[System.Serializable] public class PlayerInteraction : UnityEvent { }

public class PlayerActions : MonoBehaviour
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
    private DialogueAnimator animatedText;

    private int interactionCounter;


    private void Awake()
    {
        LeftXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("LeftHandController").GetComponent<XRRayInteractor>();
        RightXRRayInteractor = MyXROrigin.transform.GetChild(0).Find("RightHandController").GetComponent<XRRayInteractor>();

        // Directly assign Canvas component
        Canvas = MyXROrigin.transform.Find("FollowingCanvas_DialogueBox").gameObject;
        // Directly assign AnimatedText component First access the image, then the text
        animatedText = Canvas.transform.GetChild(0).Find("AnimatedText").GetComponent<DialogueAnimator>();
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
        animatedText.ReadText(DialogueText[interactionCounter], DialogueDurations[interactionCounter]);

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

    public void PerformAction()
    {
        Debug.Log($"Diana action {interactionCounter}");
        if (interactionCounter == 2)
        {
            MyXROrigin.transform.position = new Vector3(-0.5f, 0f, 1f);
            MyXROrigin.transform.Rotate(new Vector3(0, 0, 70));
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
