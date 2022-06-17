using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionsManager : MonoBehaviour
{

    public PlayerActions playerActions;

    public List<CharacterController> characters;

    public List<string> turningNames;

    [SerializeField]
    private int talkingTurn;

    public static bool hasCharacterCorFinished;


    // Start is called before the first frame update
    void Start()
    {
        hasCharacterCorFinished = true;
        playerActions.PlayerCompletedAction.AddListener(NextTurn);
    }

    // Update is called once per frame
    void Update()
    {
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

    void DoNextInteraction()
    {
        var characterTurnId = GetNextTurn(turningNames[talkingTurn]);
        if (characterTurnId == 9999)
        {
            // Wait for player action
            playerActions.PlayerInteraction.Invoke();
        }
        else
        {
            characters[characterTurnId].PerformAction();
            NextTurn();

        }
    }

    void NextTurn()
    {
        if(talkingTurn < turningNames.Count-1)
        {
            Debug.Log($"Current turn: {talkingTurn}, {turningNames[talkingTurn]}");
            talkingTurn++;
        }
        else
        {
            Debug.Log("No more actions!!");
        }
    }

    int GetNextTurn(string personTurn)
    {
        switch (personTurn)
        {
            case "Diana":
                return 9999;
            case "James":
                return 0;
            case "Rachel":
                return 1;
            default:
                break;
        }
        // Bad return
        Debug.Log("The character was not found on list");
        return 666;
    }
}
