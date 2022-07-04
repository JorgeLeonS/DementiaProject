using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public static class PostProcess_Manager
{
    public static Volume sceneVolume;
    private static Bloom bloom;
    private static float bloomIntensityMemory;

    // Sequences variables
    public static Sequence blindsOpenSequence;

    // Start is called before the first frame update
    //void Start()
    //{
    //    ChangeAmbientLightIntensity(0.1f, 0.1f);
    //    ChangeEvnironmentReflectionsIntensity(0.2f, 0.1f);
    //    try
    //    {
    //        sceneVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>();
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.LogError($"PostProcessVolume could not be found on the scene! \n" +
    //            $"Is the naming correct?\n {e}");
    //    }
    //    sceneVolume.profile.TryGet<Bloom>(out bloom);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        OpenBlinds();
    //    }
    //}

    #region DoTween methods to change ambient values
    /// <summary>
    /// The methods on this section perform a similar task. 
    /// They are sent a float for the new value to have, and a duration on seconds.
    /// These will transition from the value they have assigned on Scene, 
    /// to the newValue on the time of duration sent.
    /// </summary>

    public static Tween ChangeBloom_Intensity(Bloom bloom, float newValue, float duration = 3f)
    {
        return DOVirtual.Float(bloom.intensity.value, newValue, duration, newVal => {
            bloom.intensity.value = newVal;
        });
    }
    #endregion
}
