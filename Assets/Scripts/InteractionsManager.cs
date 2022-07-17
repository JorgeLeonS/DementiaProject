using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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

    public float DelayTimeForStartingScene = 3f;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //SceneEvents.current.completedInteraction += CheckNextInteraction;
        SceneEvents.current.completedAction += CheckNextInteraction;

        yield return new WaitForSeconds(DelayTimeForStartingScene);

        StartCoroutine(CheckNextInteraction());
    }

    private void OnDestroy()
    {
        //SceneEvents.current.completedInteraction -= CheckNextInteraction;
        SceneEvents.current.completedAction -= CheckNextInteraction;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /// <summary>
    /// Method to parse the turningNames list items. If they contain <int> or <float>,
    /// they should call an action or a delay for the detected number.
    /// </summary>
    IEnumerator CheckNextInteraction()
    {
        int repeatTimes;
        float delayTime;
        string nameInList = turningNames[characterTurn];
        if (nameInList.Contains("<"))
        {
            int firstArrowIndex = nameInList.IndexOf("<");
            string nextInteraction = nameInList.Substring(0, firstArrowIndex);
            if(nextInteraction == "Delay")
            {
                delayTime = float.Parse(nameInList.Substring(firstArrowIndex + 1, 3));
                print("starting delay");
                yield return StartCoroutine(DoNextInteraction(nextInteraction, delayTime));
                print("finished delay");
                NextTurn();
                SceneEvents.current.CompletedAction();
            }
            else
            {
                repeatTimes = int.Parse(nameInList.Substring(firstArrowIndex + 1, 1));
                for (int i = 0; i < repeatTimes; i++)
                {
                    yield return StartCoroutine(DoNextInteraction(nextInteraction));
                }
                NextTurn();
                SceneEvents.current.CompletedAction();
            }
        }
        else
        {
            //yield return DoNextInteraction(nameInList);
            yield return StartCoroutine(DoNextInteraction(nameInList));
            NextTurn();
            SceneEvents.current.CompletedAction();
            //yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// This method will decide if it's a character's or player's turn. 
    /// </summary>
    IEnumerator DoNextInteraction(string interaction, float delayTime = 0f)
    {
        print(interaction + " turn " + characterTurn);
        switch (interaction)
        {
            case "PlayerDialogue":
                yield return SceneEvents.current.PlayerDialogue();
                break;
            case "PlayerAction":
                yield return SceneEvents.current.PlayerAction();
                break;
            case "SceneAction":
                yield return SceneEvents.current.SceneAction();
                break;
            case "James":
                yield return SceneEvents.current.CharacterDialogue("James");
                break;
            case "Delay":
                yield return StartCoroutine(WaitForSeconds(delayTime));
                break;
            default:
                break;
        }
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    /// <summary>
    /// This method is called from an event when either a player or a character 
    /// have finished their action.
    /// </summary>
    void NextTurn()
    {
        if(characterTurn < turningNames.Count-1)
        {
            characterTurn++;
            Debug.Log($"Current turn: {characterTurn}, {turningNames[characterTurn]}");
        }
        else
        {
            Debug.LogWarning("No more actions!!");
        }
    }
}
