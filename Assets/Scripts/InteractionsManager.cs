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

    // Start is called before the first frame update
    IEnumerator Start()
    {
        SceneEvents.current.playerCompletedInteraction += DoNextInteraction;
        SceneEvents.current.characterCompletedInteraction += DoNextInteraction;

        yield return new WaitForSeconds(4f);

        DoNextInteraction();
    }

    private void OnDestroy()
    {
        SceneEvents.current.playerCompletedInteraction -= DoNextInteraction;
        SceneEvents.current.characterCompletedInteraction -= DoNextInteraction;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /// <summary>
    /// This method will decide if it's a character's or player's turn. 
    /// </summary>
    void DoNextInteraction()
    {
        switch (turningNames[characterTurn])
        {
            case "PlayerDialogue":
                SceneEvents.current.PlayerDialogue();
                break;
            case "PlayerAction":
                SceneEvents.current.PlayerAction();
                break;
            case "SceneAction":

            case "James":
                SceneEvents.current.CharacterDialogue("James");
                break;
            default:
                break;
        }
        NextTurn();
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
