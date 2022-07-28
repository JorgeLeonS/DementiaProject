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
        // Important so that the character movement is driven by the animation clip so the feet don't slide on the ground,
        // making the movement look more natural.
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
        // Adapted method from:
        // https://www.youtube.com/watch?v=_yuAaEbl_ns
        // Compare the next position of the nav mesh agent with its current postion
        worldDeltaPosition = navMeshAgent.nextPosition - transform.position;
        // Obtain the dot products to obtain the component in the forward and sides direction
        groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
        groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
        // Divide the time duration of the frame to get the velocity the character should move
        velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : velocity = Vector2.zero;
        // If the velocity is greater than a small number and character has not yet arrived to its destination 
        bool shouldMove = velocity.magnitude > 0.025f && navMeshAgent.remainingDistance > navMeshAgent.radius;

        // Set animator variables
        animator.SetBool("Walk", shouldMove);
        //animator.SetFloat("direction");
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
        // Correction so that the character and agent positions match.
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
            Vector3 dir = moveLocations[walkCounter - 1].position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1.5f * Time.deltaTime);
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
        // If there is no dialogue, simply wait on dialogue duration
        if (characterInteraction.DialogueText[interactionCounter] == "")
        {
            yield return new WaitForSeconds(characterInteraction.DialogueDurations[interactionCounter] + 1.0f);

        }
        // If there is no dialogue, but there is an animation
        else if (characterInteraction.DialogueText[interactionCounter] == "" && characterInteraction.AnimationName[interactionCounter] != "")
        {
            animator.SetBool(characterInteraction.AnimationName[interactionCounter], true);
            yield return new WaitForSeconds(characterInteraction.DialogueDurations[interactionCounter] + 1.0f);
            GetComponent<Animator>().SetBool(characterInteraction.AnimationName[interactionCounter], false);
        }
        // If there is a dialogue
        else
        {
            // Check for animation name, and play it
            if(characterInteraction.AnimationName[interactionCounter] != "")
                animator.SetBool(characterInteraction.AnimationName[interactionCounter], true);
            
            Canvas.GetComponent<Canvas>().enabled = true;

            // To display the text over a period of time:
            // If there is no audio on the inspector, then use the DialogueDurations list.
            if (characterInteraction.DialogueAudios[interactionCounter] == null)
            {
                animatedText.ReadText(characterInteraction.DialogueText[interactionCounter], characterInteraction.DialogueDurations[interactionCounter]);
                yield return new WaitForSeconds(characterInteraction.DialogueDurations[interactionCounter] + 1.0f);
            }
            // Else, get the duration of the audio on the DialogueAudios list.
            else
            {
                var currentClip = audioSource.clip = characterInteraction.DialogueAudios[interactionCounter];
                animatedText.ReadText(characterInteraction.DialogueText[interactionCounter], currentClip);
                audioSource.Play();
                yield return new WaitForSeconds(currentClip.length + 1.0f);
            }

            // Check for animation name, and stop it
            if (characterInteraction.AnimationName[interactionCounter] != "")
                GetComponent<Animator>().SetBool(characterInteraction.AnimationName[interactionCounter], false);
            
            Canvas.GetComponent<Canvas>().enabled = false;
        }
    }
}
