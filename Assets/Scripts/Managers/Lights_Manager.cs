using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lights_Manager
{
    //private static float duration;

    public static Tween ChangeAmbientLightIntensity(float newValue, float duration = 3f)
    {
        return DOVirtual.Float(RenderSettings.ambientIntensity, newValue, duration, newVal => {
            RenderSettings.ambientIntensity = newVal;
        });
    }

    public static Tween ChangeEnvironmentReflectionsIntensity(float newValue, float duration = 3f)
    {
        return DOVirtual.Float(RenderSettings.reflectionIntensity, newValue, duration, newVal => {
            RenderSettings.reflectionIntensity = newVal;
        });
    }

    #region Fade In and Out coroutines
    // Adapted functions from:
    // https://stackoverflow.com/questions/46419975/increase-and-decrease-light-intensity-overtime
    public static IEnumerator FadeInAndOutALight(Light lightToFade, bool fadeIn, float duration = 1f)
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
    public static IEnumerator FadeInAndOutRepeatALight(Light lightToFade, float duration = 2f)
    {
        while (true)
        {
            //Fade out
            yield return FadeInAndOutALight(lightToFade, true, duration);
            //Fade-in 
            yield return FadeInAndOutALight(lightToFade, false, duration);
        }
    }
    #endregion

}
