using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public string Name;
    public List<string> dialogueText;
    public List<AudioClip> dialogueAudios;

    public AudioSource audioSource;

    public DialogueAnimator animatedText;
    public GameObject Canvas;

    int talkCounter;

    //private DialogueTest animatedText;

    IEnumerator CorPerformAction()
    {
        //To change depending on the animation
        GetComponent<Animator>().SetBool("Wave", true);
        Canvas.GetComponent<Canvas>().enabled = true;

        var currentClip = audioSource.clip = dialogueAudios[talkCounter]; 

        animatedText.ReadText(dialogueText[talkCounter], currentClip);

        audioSource.Play();
        yield return new WaitForSeconds(currentClip.length+1.0f);


        GetComponent<Animator>().SetBool("Wave", false);
        Canvas.GetComponent<Canvas>().enabled = false;

        talkCounter++;


    }

    public void PerformAction()
    {
        StartCoroutine(CorPerformAction());
    }

    // Start is called before the first frame update
    void Start()
    {
        //animatedText = TextManager.instance.animatedText;
        Canvas.GetComponent<Canvas>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
