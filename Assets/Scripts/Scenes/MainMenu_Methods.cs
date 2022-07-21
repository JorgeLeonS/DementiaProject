using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Methods : MonoBehaviour
{
    [SerializeField] string nextLevel;
    public Light defectiveLamp_Light;
    public AudioSource defectiveLamp_Audio;
    public AudioClip bulbPop_AudioClip;

    public Canvas CanvasObject;
    public GameObject TextRevealerObject;

    /*
     * Variables for playing random piano notes on a random audio source.
     * Commented it because it sounds terrible.
    //[SerializeField]
    //private AudioClip[] pianoNotes;
    //[SerializeField]
    //private AudioSource[] sourcesForPianoNotes;
    */

    // Start is called before the first frame update
    void Start()
    {
        Lights_Manager.ChangeAmbientLightIntensity(0.1f, 0.1f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        float duration = defectiveLamp_Audio.clip.length / 2;
        StartCoroutine(Lights_Manager.FadeInAndOutRepeatALight(defectiveLamp_Light, duration));
    }

    /// <summary>
    /// Method for playing random piano notes on a random audio source.
    /// Commented it because it sounds terrible.
    /// </summary>
    //public void PlayRandomNoteOnRandomSource()
    //{
    //    AudioSource selected = sourcesForPianoNotes[Random.Range(0, sourcesForPianoNotes.Length)];
    //    AudioClip clip = pianoNotes[Random.Range(0, pianoNotes.Length)];
    //    selected.PlayOneShot(clip);
    //} 

    /// <summary>
    /// Action to be called on the "Start Experience" button, on the scene.
    /// </summary>
    public void StartExperienceAction()
    {
        defectiveLamp_Audio.Pause();
        defectiveLamp_Audio.loop = false;
        StopAllCoroutines();
        StartCoroutine(BulbPop());
    }

    #region Scene specific methods
    /// <summary>
    /// This will start have the effect to the Bulb in scene increases its intensity,
    /// and then it pops. Light and audio are managed here.
    /// </summary>
    IEnumerator BulbPop()
    {
        yield return StartCoroutine(Lights_Manager.FadeInAndOutALight(defectiveLamp_Light, true));
        defectiveLamp_Light.intensity = 0;
        defectiveLamp_Audio.clip = bulbPop_AudioClip;
        defectiveLamp_Audio.Play();
        TextRevealerObject.GetComponent<TextRevealer>().Unreveal();
        yield return new WaitForSeconds(0.5f);
        MenuControl.LoadLevel(nextLevel);
    }
    #endregion
}
