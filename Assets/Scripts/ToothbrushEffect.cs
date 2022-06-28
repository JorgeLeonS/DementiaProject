using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToothbrushEffect : MonoBehaviour
{
    [SerializeField] List<float> timers = new List<float>();
    [SerializeField] GameObject helpButton;

    float timer;
    int indexSequence = 0;

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
        helpButton.gameObject.SetActive(false);
        timer = timers[indexSequence];
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            Debug.Log($"time left: {timer}");
        }
        else
        {
            // if index is last dont reactivate.
            helpButton.gameObject.SetActive(true);
        }
    }

    private void NextSequence()
    {
        indexSequence++;
        helpButton.gameObject.SetActive(false);
        if (indexSequence < timers.Count)
        {
            timer = timers[indexSequence];
        }
    }

    // todo text "help is coming, but keep trying to find the toothbrush"
    // todo make toothbrush to start to appear but difficult to grab
}
