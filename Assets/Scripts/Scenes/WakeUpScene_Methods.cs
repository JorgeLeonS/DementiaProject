using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

/// <summary>
/// 
/// </summary>
public class WakeUpScene_Methods : MonoBehaviour
{
    public static Volume sceneVolume;
    private static Bloom bloom;

    // Sequences variables
    public static Sequence blindsOpenSequence;

    // Start is called before the first frame update
    void Start()
    {
        Lights_Manager.ChangeAmbientLightIntensity(0.1f, 0.1f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        try
        {
            sceneVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"PostProcessVolume could not be found on the scene! \n" +
                $"Is the naming correct?\n {e}");
        }
        sceneVolume.profile.TryGet<Bloom>(out bloom);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Scene specific methods
    /// <summary>
    /// When the 
    /// </summary>
    public static IEnumerator OpenBlinds()
    {
        var hasSequenceCompleted = false;
        blindsOpenSequence = DOTween.Sequence();
        Lights_Manager.ChangeAmbientLightIntensity(1, 0.5f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        blindsOpenSequence.Append(PostProcess_Manager.ChangeBloom_Intensity(bloom, 50, 3f));
        blindsOpenSequence.AppendInterval(1f);
        yield return new WaitForSeconds(4f);
        blindsOpenSequence.Append(PostProcess_Manager.ChangeBloom_Intensity(bloom, 0.1f, 6f));
        blindsOpenSequence.OnComplete(() => {
            hasSequenceCompleted = true;
        });
        yield return new WaitUntil(() => hasSequenceCompleted);
    }
    #endregion
}
