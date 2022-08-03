using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialSection;
    public GameObject StartExperienceSection;

    public Button previousButton;
    public Button nextButton;

    public TMP_Text textExplanation;
    public GameObject videoExplanation;

    public List<string> textExplanations;
    public List<VideoClip> videoExplanations;

    [SerializeField]
    int explanationCounter = 0;

    public void NextExplanation()
    {
        explanationCounter++;
        if (explanationCounter >= textExplanations.Count)
        {
            SkipExplanation();
            explanationCounter = -1;
            NextExplanation();
        }    
        else
        {
            previousButton.interactable = true;
            textExplanation.text = textExplanations[explanationCounter];
            videoExplanation.GetComponent<VideoPlayer>().clip = videoExplanations[explanationCounter];
        }
    }

    public void PreviousExplanation()
    {
        explanationCounter--;
        if (explanationCounter >= 0)
        {
            if(explanationCounter == 0)
                previousButton.interactable = false;

            textExplanation.text = textExplanations[explanationCounter];
            videoExplanation.GetComponent<VideoPlayer>().clip = videoExplanations[explanationCounter];
        }else
            previousButton.interactable = false;
    }

    public void SkipExplanation()
    {
        TutorialSection.SetActive(false);
        //StartExperienceSection.SetActive(true);
    }

    public void BackToExplanations()
    {
        TutorialSection.SetActive(true);
        StartExperienceSection.SetActive(false);

        explanationCounter = textExplanations.Count - 1;
        previousButton.interactable = true;
        textExplanation.text = textExplanations[explanationCounter];
        videoExplanation.GetComponent<VideoPlayer>().clip = videoExplanations[explanationCounter];
    }

    public void StartExperience()
    {
        MenuControl.LoadLevel("MainMenu2");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
