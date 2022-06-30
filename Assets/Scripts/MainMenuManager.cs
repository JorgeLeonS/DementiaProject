using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string nextLevel;
    public Light defectiveLamp_Light;
    public AudioSource defectiveLamp_Audio;
    public AudioClip bulbPop_AudioClip;

    public Canvas CanvasObject;
    public GameObject TextRevealerObject;

    MenuControl menuControl;

    private float duration;

    // Start is called before the first frame update
    void Start()
    {
        duration = defectiveLamp_Audio.clip.length / 2;
        StartCoroutine(FadeInAndOutRepeat(defectiveLamp_Light));
        menuControl = gameObject.GetComponent<MenuControl>();
    }

    public void AssignUILayerToSlices()
    {
        Transform sliced = CanvasObject.transform.Find(TextRevealerObject.name + "_sliced");
        if (sliced != null)
        {
            sliced.gameObject.layer = 2;
            foreach (Transform child in sliced)
            {
                child.gameObject.layer = 2;
            }
        }
    }

    private void ChangeScene()
    {
        if (menuControl != null)
            menuControl.LoadLevel(nextLevel);
        else
            Debug.LogError("Add the MenuControl script to the scene");
    }

    private void Update()
    {
        //AssignUILayerToSlices();
    }

    public void Buttonpressed()
    {
        defectiveLamp_Audio.Pause();
        defectiveLamp_Audio.loop = false;
        StopAllCoroutines();
        duration = 1f;
        StartCoroutine(BulbPop());
    }

    IEnumerator BulbPop()
    {
        yield return StartCoroutine(FadeInAndOut(defectiveLamp_Light, true));
        defectiveLamp_Light.intensity = 0;
        defectiveLamp_Audio.clip = bulbPop_AudioClip;
        defectiveLamp_Audio.Play();
        yield return new WaitForSeconds(0.5f);
        ChangeScene();
    }

    #region Fade In and Out coroutines
    // Adapted functions from:
    // https://stackoverflow.com/questions/46419975/increase-and-decrease-light-intensity-overtime
    IEnumerator FadeInAndOut(Light lightToFade, bool fadeIn)
    {
        float minLuminosity = 0.03f; // min intensity
        float maxLuminosity = 0.3f; // max intensity

        float counter = 0f;

        //Set Values depending on if fadeIn or fadeOut
        float a, b;

        if (fadeIn)
        {
            a = minLuminosity;
            b = maxLuminosity;
        }
        else
        {
            a = maxLuminosity;
            b = minLuminosity;
        }

        float currentIntensity = lightToFade.intensity;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            lightToFade.intensity = Mathf.Lerp(a, b, counter / duration);

            yield return null;
        }
    }

    //Fade in and out forever
    IEnumerator FadeInAndOutRepeat(Light lightToFade)
    {
        while (true)
        {
            //Fade out
            yield return FadeInAndOut(lightToFade, true);
            //Fade-in 
            yield return FadeInAndOut(lightToFade, false);
        }
    }
    #endregion
}
