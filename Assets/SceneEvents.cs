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
    public IEnumerator PlayerDialogue()
    {
        playerDialogueHanlder pD = playerDialogue;
        if (playerDialogue != null)
        {
            yield return StartCoroutine(playerDialogue());
        }
    }

    public delegate IEnumerator playerActionHanlder();
    public event playerActionHanlder playerAction;
    public IEnumerator PlayerAction()
    {
        playerActionHanlder pA = playerAction;
        if (playerAction != null)
        {
            yield return StartCoroutine(playerAction());
        }
    }
    #endregion

    #region Character events
    public delegate IEnumerator characterDialogueHanlder(string characterName);
    public event characterDialogueHanlder characterDialogue;
    public IEnumerator CharacterDialogue(string characterName)
    {
        characterDialogueHanlder chD = characterDialogue;
        if (characterDialogue != null)
        {
            yield return StartCoroutine(characterDialogue(characterName));
        }
    }
    #endregion

    #region Environment events
    public delegate IEnumerator sceneActionHandler();
    public event sceneActionHandler sceneAction;
    public IEnumerator SceneAction()
    {
        sceneActionHandler hA = sceneAction;
        if(sceneAction != null)
        {
            yield return StartCoroutine(sceneAction());
        }
    }
    #endregion

    // Should not use this method
    public event Action completedInteraction;
    public void CompletedInteraction()
    {
        if (completedInteraction != null)
        {
            completedInteraction();
        }
    }

    public delegate IEnumerator completedActionHandler();
    public event completedActionHandler completedAction;
    public void CompletedAction()
    {
        completedActionHandler cA = completedAction;
        if (completedAction != null)
        {
            StartCoroutine(completedAction());
        }
    }
}
