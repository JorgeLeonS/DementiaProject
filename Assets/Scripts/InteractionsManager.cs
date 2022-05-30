using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionsManager : MonoBehaviour
{
    public static InteractionsManager instance;

    public DialogueAnimator animatedText;

    public CharacterData characters;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        characters.canvas.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            characters.canvas.GetComponent<Canvas>().enabled = true;
            characters.GetComponent<Animator>().SetTrigger("StartWaving");
            animatedText.ReadText(characters.dialogueText[0], characters.dialogueAudios[0]);
        }
    }
}
