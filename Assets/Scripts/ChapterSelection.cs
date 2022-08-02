using UnityEngine;
using UnityEngine.UI;

public class ChapterSelection : MonoBehaviour
{
    [SerializeField] Button wakeUpbutton;
    [SerializeField] Button toothbrushButton;
    [SerializeField] Button pillsButton;

    private void OnEnable()
    {
        wakeUpbutton.onClick.AddListener(GoToWakeUpScene);
        toothbrushButton.onClick.AddListener(GoToToothbrushScene);
        pillsButton.onClick.AddListener(GoToPillsScene);
    }

    private void OnDisable()
    {
        wakeUpbutton.onClick.RemoveAllListeners();
        toothbrushButton.onClick.RemoveAllListeners();
        pillsButton.onClick.RemoveAllListeners();
    }

    private void GoToWakeUpScene()
    {
        MenuControl.LoadLevel("WakeUpScene2");
    }

    private void GoToToothbrushScene()
    {
        MenuControl.LoadLevel("ToothbrushScene");
    }

    private void GoToPillsScene()
    {
        MenuControl.LoadLevel("PillsScene");
    }
}
