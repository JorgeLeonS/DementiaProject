using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionsManager : MonoBehaviour
{
    public static InteractionsManager instance;

    public DialogueAnimator animatedText;

    public List<CharacterController> characters;

    public List<string> turningNames = new List<string>
    {
        "TBot", "James", "Juana", "James"
    };

    private int talkingTurn;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            characters[GetNextTurn(turningNames[talkingTurn])].PerformAction();
            talkingTurn++;
        }
    }

    int GetNextTurn(string personTurn)
    {
        switch (personTurn)
        {
            case "James":
                return 0;
            case "Rachel":
                return 1;
            case "TBot":
                return 2;
            default:
                break;
        }
        return 0;
    }
}
