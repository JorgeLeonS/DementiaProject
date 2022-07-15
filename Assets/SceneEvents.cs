using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the event calls on the scene.
/// </summary>
public class SceneEvents : MonoBehaviour
{
    public static SceneEvents current;

    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
    }

    #region Player events
    public delegate IEnumerator playerDialogueHanlder();
    public event playerDialogueHanlder playerDialogue;
    public void PlayerDialogue()
    {
        playerDialogueHanlder pD = playerDialogue;
        if (playerDialogue != null)
        {
            StartCoroutine(playerDialogue());
        }
    }

    public delegate IEnumerator playerActionHanlder();
    public event playerActionHanlder playerAction;
    public void PlayerAction()
    {
        playerActionHanlder pA = playerAction;
        if (playerAction != null)
        {
            StartCoroutine(playerAction());
        }
    }

    public event Action playerCompletedInteraction;
    public void PlayerCompletedInteraction()
    {
        if (playerCompletedInteraction != null)
        {
            playerCompletedInteraction();
        }
    }
    #endregion

    #region Character events
    public delegate IEnumerator characterDialogueHanlder(string characterName);
    public event characterDialogueHanlder characterDialogue;
    public void CharacterDialogue(string characterName)
    {
        characterDialogueHanlder chD = characterDialogue;
        if (characterDialogue != null)
        {
            StartCoroutine(characterDialogue(characterName));
        }
    }

    public event Action characterCompletedInteraction;
    public void CharacterCompletedInteraction()
    {
        if (characterCompletedInteraction != null)
        {
            characterCompletedInteraction();
        }
    }
    #endregion
}
