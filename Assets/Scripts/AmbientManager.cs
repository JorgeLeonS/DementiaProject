using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

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
            Debug.LogError($"sceneVolume could not be found on the scene! \n\n {e}");
        }
        aE = sceneVolume.profile.GetSetting<AutoExposure>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenBlinds();
            //ChangeAE_MinEV(-9f);
        }
    }

    // Sequences
    public static void OpenBlinds()
    {
        blindsOpenSequence = DOTween.Sequence();
        blindsOpenSequence.Append(ChangeAmbientLightIntensity(1, 0.5f));
        blindsOpenSequence.Append(ChangeAE_MinEV(-9, 0.5f));
        blindsOpenSequence.Append(ChangeAE_MaxEV(-9, 0.5f));
        blindsOpenSequence.AppendInterval(1f);
        blindsOpenSequence.Append(ChangeAE_MaxEV(0, 0.2f));
        blindsOpenSequence.Append(ChangeAE_MinEV(0, 1f));
        //StartCoroutine(Cor_OpenBlinds());
    }

    // DoTween methods to change ambient values
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

    
}
