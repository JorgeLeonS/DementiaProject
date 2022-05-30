using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterData : MonoBehaviour
{
    public List<string> dialogueText;
    public List<AudioSource> dialogueAudios;
    public GameObject canvas;

    //private DialogueTest animatedText;

    // Start is called before the first frame update
    void Start()
    {
        //animatedText = TextManager.instance.animatedText;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
