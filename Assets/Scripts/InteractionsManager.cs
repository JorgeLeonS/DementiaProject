using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class used to define whose turn it is to have an interaction.
/// Because the project was initially thought as a story based experience, 
/// there was an understanding that we would have multiple characters on a scene, walking, talking and having actions.
/// The current scope of the project, left only with one charcater and one player to be had on scene.
/// However, this script can be adapted so that it can call different characters.
/// It communicates with <see cref="CharacterController"/> and <see cref="PlayerController"/> letting them know, when to do something.
/// </summary>
public class InteractionsManager : MonoBehaviour
{

    public PlayerController playerController;

    public List<CharacterController> characters;

    public List<string> turningNames;

    [SerializeField]
    private int characterTurn;

    // TODO change to an Event
    public static bool hasCharacterCorFinished;


    // Start is called before the first frame update
    void Start()
    {
        hasCharacterCorFinished = true;
        playerController.PlayerCompletedInteraction.AddListener(NextTurn);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO Remove, it's just for testing purposes.
        // The DoNextInteraction method needs to be called when an action has finished. (NextTurn) event call.
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (hasCharacterCorFinished)
            {
                DoNextInteraction();
            }
            else
            {
                Debug.Log("Character has not finished its action");
            }
        }
    }

    /// <summary>
    /// This method will decide if it's a character's or player's turn. 
    /// </summary>
    void DoNextInteraction()
    {
        switch (turningNames[characterTurn])
        {
            case "Diana":
                playerController.PlayerDialogue.Invoke();
                break;
            case "DianaAction":
                playerController.PlayerAction.Invoke();
                break;
            case "James":
                characters[characterTurn].PerformAction();
                /// TODO Change this to be called from the character event
                /// Related to <see cref="hasCharacterCorFinished"/>
                /// Find a similar implementation to <see cref="PlayerController.PlayerCompletedInteraction"/> action
                NextTurn();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// This method is called from an event when either a player or a character 
    /// have finished their action.
    /// </summary>
    void NextTurn()
    {
        if(characterTurn < turningNames.Count-1)
        {
            Debug.Log($"Current turn: {characterTurn}, {turningNames[characterTurn]}");
            characterTurn++;
        }
        else
        {
            Debug.Log("No more actions!!");
        }
    }
}
