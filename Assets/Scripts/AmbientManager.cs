using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

/// <summary>
/// Using the DoTween package, different methods were created so that every individual value
/// on a Post Processing volume can be modified and transitioned over time.
/// These can later be called on DoTween Sequences, which make the effects happen one after another.
/// </summary>
public class AmbientManager : MonoBehaviour
{
    public static PostProcessVolume sceneVolume;
    private static AutoExposure aE;

    // Sequences variables
    public static Sequence blindsOpenSequence;

    // Start is called before the first frame update
    void Start()
    {
        ChangeAmbientLightIntensity(0.1f, 0.1f);
        try
        {
            sceneVolume = GameObject.Find("PostProcessVolume").GetComponent<PostProcessVolume>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"PostProcessVolume could not be found on the scene! \n" +
                $"Is the naming correct?\n {e}");
        }
        aE = sceneVolume.profile.GetSetting<AutoExposure>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenBlinds();
        }
    }

    #region DoTween Sequences
    /// <summary>
    /// These will be used to call the different DoTween methods in form of Sequences.
    /// All these methods contain two parameters, the first one for the new vale to assign, 
    /// and the second for the duration the transition will last.
    /// E.g. Calling <see cref="ChangeAE_MinEV(float, float)"/> 
    /// such as, ChangeAE_MinEV(-9f, 0.5f);
    /// Will assign the new MinEV value to -9 on 0.5 seconds.
    /// </summary>
    public static void OpenBlinds()
    {
        //ChangeAmbientLightIntensity(1, 0.5f).OnComplete(() =>
        //ChangeAE_MinEV(-9, 0.5f).OnComplete(() =>
        //ChangeAE_MaxEV(-9, 0.5f).OnComplete(() =>
        //// Missing 1 sec delay
        //ChangeAE_MaxEV(0, 0.2f).OnComplete(() =>
        //ChangeAE_MinEV(0, 1f)))));

        blindsOpenSequence = DOTween.Sequence();
        blindsOpenSequence.Append(ChangeAmbientLightIntensity(1, 0.5f));
        blindsOpenSequence.Append(ChangeAE_MinEV(-9, 0.5f));
        blindsOpenSequence.Append(ChangeAE_MaxEV(-9, 0.5f));
        blindsOpenSequence.AppendInterval(1f);
        blindsOpenSequence.Append(ChangeAE_MaxEV(0, 0.2f));
        blindsOpenSequence.Append(ChangeAE_MinEV(0, 1f));
    }
    #endregion

    #region DoTween methods to change ambient values
    /// <summary>
    /// The methods on this section perform a similar task. 
    /// They are sent a float for the new value to have, and a duration on seconds.
    /// These will transition from the value they have assigned on Scene, 
    /// to the newValue on the time of duration sent.
    /// </summary>
    public static Tween ChangeAmbientLightIntensity(float newValue, float duration = 3f)
    {
        return DOVirtual.Float(RenderSettings.ambientIntensity, newValue, duration, newVal => {
            RenderSettings.ambientIntensity = newVal;
        });
    }

    public static Tween ChangeAE_MinEV(float newValue, float duration = 3f)
    {
        var minEV = aE.minLuminance;
        return DOVirtual.Float(minEV.value, newValue, duration, newVal => {
            minEV.value = newVal;
        });
    }

    public static Tween ChangeAE_MaxEV(float newValue, float duration = 3f)
    {
        var maxEV = aE.maxLuminance;
        return DOVirtual.Float(maxEV.value, newValue, duration, newVal => {
            maxEV.value = newVal;
        });
    }
    #endregion

}
