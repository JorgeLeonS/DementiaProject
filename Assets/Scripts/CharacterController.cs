using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    //[SerializeField] public string Name;
    //public List<string> dialogueText;
    //public List<AudioClip> dialogueAudios;

    [SerializeField] private SO_CharactersInteractions characterInteraction;

    public AudioSource audioSource;

    public DialogueAnimator animatedText;
    public GameObject Canvas;

    int interactionCounter;

    private Animator animator;

    // Does not work if it does not have the serializeField??? or being public??
    //[SerializeField] 
    [HideInInspector]
    public List<Transform> moveLocations;
    private NavMeshAgent navMeshAgent;


    //private DialogueTest animatedText;

    void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get all waypoints on a list
        Transform CharacterWaypoints = GameObject.FindGameObjectWithTag("Waypoints").transform.Find(characterInteraction.Name);
        foreach (Transform wp in CharacterWaypoints)
        {
            moveLocations.Add(wp);
        }
    }

    IEnumerator Cor_PerformAction()
    {
        if (characterInteraction.moveToNextLocation[interactionCounter])
        {
            animator.SetBool("Walk", true);
            navMeshAgent.destination = moveLocations[interactionCounter].position;

            yield return new WaitUntil(() => HasCharacterReachedDestination());

            animator.SetBool("Walk", false);
        }

        // To change depending on the animation
        animator.SetBool(characterInteraction.animation[interactionCounter], true);
        Canvas.GetComponent<Canvas>().enabled = true;

        var currentClip = audioSource.clip = characterInteraction.dialogueAudios[interactionCounter]; 

        animatedText.ReadText(characterInteraction.dialogueText[interactionCounter], currentClip);

        audioSource.Play();
        yield return new WaitForSeconds(currentClip.length+1.0f);


        GetComponent<Animator>().SetBool(characterInteraction.animation[interactionCounter], false);
        Canvas.GetComponent<Canvas>().enabled = false;

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
