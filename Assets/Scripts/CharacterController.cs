using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private SO_CharactersInteractions characterInteraction;

    // Necessary components
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private GameObject Canvas;
    private DialogueAnimator animatedText;
    private AudioSource audioSource;

    int interactionCounter;

    // Does not work if it does not have the serializeField??? or being public??
    //[SerializeField] 
    [HideInInspector]
    public List<Transform> moveLocations;

    void Awake()
    {
        // Directly assign Animator component
        animator = GetComponent<Animator>();
        // Directly assign navMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Directly assign Canvas component
        Canvas = transform.Find("Canvas_DialogueBox").gameObject;
        // Directly assign AnimatedText component First access the image, then the text
        animatedText = Canvas.transform.GetChild(0).Find("AnimatedText").GetComponent<DialogueAnimator>();

        // Directly assign AudioSource component
        audioSource = transform.Find("AudioSource").GetComponent<AudioSource>();

        // Get all waypoints on a list
        Transform CharacterWaypoints = GameObject.FindGameObjectWithTag("Waypoints").transform.Find(characterInteraction.Name);
        foreach (Transform wp in CharacterWaypoints)
        {
            moveLocations.Add(wp);
        }
    }

    IEnumerator Cor_MoveToNextLocation()
    {
        animator.SetBool("Walk", true);
        navMeshAgent.destination = moveLocations[interactionCounter].position;

        yield return new WaitUntil(() => HasCharacterReachedDestination());

        animator.SetBool("Walk", false);
    }

    IEnumerator Cor_NextDialogue()
    {
        animator.SetBool(characterInteraction.animation[interactionCounter], true);
        Canvas.GetComponent<Canvas>().enabled = true;

        var currentClip = audioSource.clip = characterInteraction.dialogueAudios[interactionCounter];

        animatedText.ReadText(characterInteraction.dialogueText[interactionCounter], currentClip);

        audioSource.Play();
        yield return new WaitForSeconds(currentClip.length + 1.0f);


        GetComponent<Animator>().SetBool(characterInteraction.animation[interactionCounter], false);
        Canvas.GetComponent<Canvas>().enabled = false;
    }

    IEnumerator Cor_PerformAction()
    {
        // If the character has MoveToNextLocation set to true 
        if (characterInteraction.moveToNextLocation[interactionCounter])
        {
            yield return Cor_MoveToNextLocation();
        }

        yield return Cor_NextDialogue();
        
        interactionCounter++;

    }

    // Function from: https://gist.github.com/DataGreed/df0c008be1f9269d5160af413e939843
    public bool HasCharacterReachedDestination()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void PerformAction()
    {
        StartCoroutine(Cor_PerformAction());
    }

    // Start is called before the first frame update
    void Start()
    {
        //animatedText = TextManager.instance.animatedText;
        Canvas.GetComponent<Canvas>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
