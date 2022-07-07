using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToothbrushEffect : MonoBehaviour
{
    [SerializeField] List<float> timers = new List<float>();
    [SerializeField] List<string> prompts = new List<string>();
    [SerializeField] GameObject helpButton;
    [SerializeField] GameObject textSign;
    [SerializeField] Transform toothbrush;

    List<Material> toothbrushMats = new List<Material>();
    float timer;
    int indexSequence = 0;
    float toothbrushtimer = 0f;

    private void OnEnable()
    {
        helpButton.GetComponent<Button>().onClick.AddListener(NextSequence);
    }

    private void OnDisable()
    {
        helpButton.GetComponent<Button>().onClick.RemoveListener(NextSequence);
    }

    private void Start()
    {
        timer = timers[indexSequence];
        helpButton.gameObject.SetActive(false);
        SetSequence();
        textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
        textSign.SetActive(true);
        foreach (Material material in toothbrush.GetComponent<Renderer>().materials)
        {
            toothbrushMats.Add(material);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (indexSequence < timers.Count - 1)
            {
                helpButton.gameObject.SetActive(true);
                textSign.SetActive(false);
            }
        }
        else
        {
            Debug.Log($"time left: {timer}");
        }
        if(indexSequence == 1)
        {
            toothbrushtimer += Time.deltaTime;
            if(toothbrushtimer < 5f)
            {
                foreach (Material material in toothbrushMats)
                {
                    material.SetFloat("_Power", toothbrushtimer * 0.1f);
                }
            }
        }
    }

    private void NextSequence()
    {
        indexSequence++;
        SetSequence();
        if (indexSequence < timers.Count)
        {
            
            helpButton.gameObject.SetActive(false);
            textSign.GetComponentInChildren<TextMeshProUGUI>().text = prompts[indexSequence];
            timer = timers[indexSequence];
            textSign.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    

    private void SetSequence()
    {
        switch (indexSequence)
        {
            case 0:
                toothbrush.gameObject.SetActive(false);
                break;
            case 1:
                toothbrush.gameObject.SetActive(true);// activate refraction effect
                // set tootbrush active
                break;
            case 2:
                // wait for james
                // fade out and fade in
                // change to pill sequence
                break;
            default:
                Debug.Log("Out of range");
                break;
        }
    }

    // todo make toothbrush to start to appear but difficult to grab
}
