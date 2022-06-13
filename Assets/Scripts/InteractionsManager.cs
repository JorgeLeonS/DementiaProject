using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionsManager : MonoBehaviour
{

    public PlayerActions playerActions;

    public List<CharacterController> characters;

    public List<string> turningNames = new List<string>
    {
        "TBot", "Diana", "James", "Juana", "James"
    };

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
            else
            {
                Debug.Log("Character has not finished its action");
            }
            
            
        }
    }

    void NextTurn()
    {
        Debug.Log("NextTurn");
        talkingTurn++;
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
            case "TBot":
                return 2;
            default:
                break;
        }
        // Bad return
        Debug.Log("The character was not found on list");
        return 666;
    }
}
