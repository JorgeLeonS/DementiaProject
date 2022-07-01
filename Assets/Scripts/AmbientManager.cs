using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

/// <summary>
/// Using the DoTween package, different methods were created so that every individual value
/// on a Post Processing volume can be modified and transitioned over time.
/// These can later be called on DoTween Sequences, which make the effects happen one after another.
/// </summary>
public class AmbientManager : MonoBehaviour
{
    public static Volume sceneVolume;
    private static Bloom bloom;
    private static float bloomIntensityMemory;

    // Sequences variables
    public static Sequence blindsOpenSequence;

    // Start is called before the first frame update
    void Start()
    {
        ChangeAmbientLightIntensity(0.1f, 0.1f);
        ChangeEvnironmentReflectionsIntensity(0.2f, 0.1f);
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
    public static IEnumerator OpenBlinds()
    {
        var hasSequenceCompleted = false;
        blindsOpenSequence = DOTween.Sequence();
        ChangeAmbientLightIntensity(1, 0.5f);
        ChangeEvnironmentReflectionsIntensity(0.2f, 0.1f);
        blindsOpenSequence.Append(ChangeBloom_Intensity(50, 3f));
        bloomIntensityMemory = 50;
        blindsOpenSequence.AppendInterval(1f);
        blindsOpenSequence.Append(ChangeBloom_Intensity(0.1f, 6f));
        blindsOpenSequence.OnComplete(() => {
            bloomIntensityMemory = 0;
            hasSequenceCompleted = true;
        });
        yield return new WaitUntil(() => hasSequenceCompleted);
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

    public static Tween ChangeEvnironmentReflectionsIntensity(float newValue, float duration = 3f)
    {
        return DOVirtual.Float(RenderSettings.reflectionIntensity, newValue, duration, newVal => {
            RenderSettings.reflectionIntensity = newVal;
        });
    }

    public static Tween ChangeBloom_Intensity(float newValue, float duration = 3f)
    {
        return DOVirtual.Float(bloomIntensityMemory, newValue, duration, newVal => {
            bloom.intensity.value = newVal;
        });
    }
    #endregion

}
