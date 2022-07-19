using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

/// <summary>
/// This class works with what the <see cref="CharacterData"/> class gives.
/// This one, different to PlayerController (Singletons), should not be managed through events, 
/// because of the possibility of adding multiple character on one scene.
/// </summary>
public class CharacterController : MonoBehaviour
{
    //[SerializeField] private SO_CharactersInteractions characterInteraction;

    // Necessary components
    private CharacterData characterInteraction;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 worldDeltaPosition;
    private Vector2 groundDeltaPosition;
    private Vector2 velocity = Vector2.zero;

    private GameObject Canvas;
    private DialogueAnimator animatedText;
    private AudioSource audioSource;

    private Transform CharacterWaypoints;

    int interactionCounter;
    int walkCounter;

    // Does not work if it does not have the serializeField??? or being public??
    //[SerializeField] 
    [HideInInspector]
    public List<Transform> moveLocations;

    /// <summary>
    /// Method where all the necessary components are added.
    /// Most of them are added by type, but it is important to have an exact match on the names on those that depend on it.
    /// </summary>
    void Awake()
    {
        // Directly assign CharacterData script
        characterInteraction = GetComponent<CharacterData>();

        // Directly assign Animator component
        animator = GetComponent<Animator>();

        // Directly assign navMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Position of the NavMeshAQgent is decoupled from the position of the character
        navMeshAgent.updatePosition = false;

        // Directly assign Canvas component
        Canvas = transform.Find("Canvas_DialogueBox").gameObject;
        // Directly assign AnimatedText component First access the image, then the text
        animatedText = Canvas.transform.GetChild(0).Find("AnimatedText").GetComponent<DialogueAnimator>();

        // Directly assign AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Get all waypoints on a list
        CharacterWaypoints = GameObject.FindGameObjectWithTag("Waypoints").transform.Find(characterInteraction.Name);
        foreach (Transform wp in CharacterWaypoints)
        {
            moveLocations.Add(wp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneEvents.current.characterDialogue += Cor_PerformAction;

        //animatedText = TextManager.instance.animatedText;
        Canvas.GetComponent<Canvas>().enabled = false;
    }

    private void Update()
    {
        // Control all the velocity 
        worldDeltaPosition = navMeshAgent.nextPosition - transform.position;
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        bool shouldMove = velocity.magnitude > 0 && navMeshAgent.remainingDistance > navMeshAgent.radius;

        // Set animator variables
        animator.SetBool("Walk", shouldMove);
        animator.SetFloat("velX", velocity.x);
        animator.SetFloat("velY", velocity.y);
    }

    private void OnDestroy()
    {
        SceneEvents.current.characterDialogue -= Cor_PerformAction;
    }

    /// <summary>
    /// Main coroutine that calls the different actions to be done by a characte.
    /// Managed mostly by interactionCounter.
    /// </summary>
    /// <returns></returns>
    IEnumerator Cor_PerformAction(string name)
    {
        if(name == characterInteraction.Name)
        {
            if (interactionCounter >= characterInteraction.DialogueText.Count)
            {
                yield return new WaitForSeconds(3f);
                Debug.LogWarning($"Bad action, the character, {characterInteraction.Name} has no more actions!");
                //SceneEvents.current.CompletedAction();
            }
            else
            {
                // If the character has MoveToNextLocation set to true 
                if (characterInteraction.MoveToNextLocation[interactionCounter])
                {
                    yield return Cor_MoveToNextLocation();
                }

                yield return Cor_NextDialogue();
                interactionCounter++;
                //SceneEvents.current.CompletedAction();
            }
        }
    }

    /// <summary>
    /// Coroutine that makes the character walk to the next waypoint.
    /// </summary>
    IEnumerator Cor_MoveToNextLocation()
    {
        navMeshAgent.destination = moveLocations[walkCounter].position;
        walkCounter++;

        yield return new WaitUntil(() => HasCharacterReachedDestination());

        //animator.SetBool("Walk", false);
    }

    private void OnAnimatorMove()
    {
        transform.position = navMeshAgent.nextPosition;
    }

    /// <summary>
    /// Method to determine if the character has reached its navmesh destination.
    /// </summary>
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

    /// <summary>
    /// Coroutine that makes the character have a dialogue.
    /// Can also call an animation and an audio.
    /// </summary>
    IEnumerator Cor_NextDialogue()
    {
        animator.SetBool(characterInteraction.AnimationName[interactionCounter], true);
        Canvas.GetComponent<Canvas>().enabled = true;

        if(characterInteraction.DialogueAudios[interactionCounter] == null)
        {
            animatedText.ReadText(characterInteraction.DialogueText[interactionCounter], characterInteraction.DialogueDurations[interactionCounter]);
            yield return new WaitForSeconds(characterInteraction.DialogueDurations[interactionCounter] + 1.0f);
        }
        else
        {
            var currentClip = audioSource.clip = characterInteraction.DialogueAudios[interactionCounter];
            animatedText.ReadText(characterInteraction.DialogueText[interactionCounter], currentClip);
            audioSource.Play();
            yield return new WaitForSeconds(currentClip.length + 1.0f);
        }

        GetComponent<Animator>().SetBool(characterInteraction.AnimationName[interactionCounter], false);
        Canvas.GetComponent<Canvas>().enabled = false;
    }
}
