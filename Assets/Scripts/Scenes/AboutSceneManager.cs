using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutSceneManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource aboutDialogue;

    [SerializeField]
    private TextRevealer aboutText;

    IEnumerator Start()
    {
        SceneEvents.current.sceneAction += SetSequence;

        yield return new WaitForSeconds(2f);

        aboutText.Reveal();
        aboutDialogue.Play();
    }

    private void OnDestroy()
    {
        SceneEvents.current.sceneAction -= SetSequence;
    }

    IEnumerator SetSequence()
    {
        yield return null;
        MenuControl.LoadLevel("MainMenuTutorial");
    }

    
}
