using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutSceneManager : MonoBehaviour
{
    void Start()
    {
        SceneEvents.current.sceneAction += SetSequence;
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
