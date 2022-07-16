using UnityEngine;
using DG.Tweening;
using UnityEditor;

/// <summary>
/// This class contains methods that change the settings of a Material made from the Heat Shader.
/// </summary>
public class DistortionEffect : MonoBehaviour
{
    [SerializeField] Renderer heatMaterial;
    [SerializeField] float maxStrength;
    [SerializeField] float maxSpeed;
    [Range(0.01f, 5f)]
    [SerializeField] float timeTransitionSettings = 0.5f;

    float strength;
    float speed;

    private void Start()
    {
        ResetEffect();
    }

    private void ResetEffect()
    {
        heatMaterial.material.SetFloat("_Strength", 0f);
        heatMaterial.material.SetFloat("_Speed", 0f);
    }

    /// <summary>
    /// This method creates a Tween to start the heat effect settings.
    /// </summary>
    public void StartTextDistortion()
    {
        ChangeDistortionStrenghtSettings(timeTransitionSettings, true);
        ChangeDistortionSpeedSettings(timeTransitionSettings, true);
    }

    /// <summary>
    /// This method creates a Tween to reverse the heat effect settings.
    /// </summary>
    public void EndTextDistortion()
    {
        ChangeDistortionSpeedSettings(timeTransitionSettings, false);
        ChangeDistortionStrenghtSettings(timeTransitionSettings, false);
    }

    private Tween ChangeDistortionStrenghtSettings(float duration, bool start)
    {
        float fromValue;
        float toValue;
        // if function is set to start effect
        if (start)
        {
            fromValue = 0f;
            toValue = maxStrength;
        }
        // if !start means it is reversing the effect.
        else
        {
            fromValue = maxStrength;
            toValue = 0f;
        }
        
        return DOVirtual.Float(fromValue, toValue, duration, newVal => {
            strength = newVal;
            heatMaterial.material.SetFloat("_Strength", strength);
        });
    }

    private Tween ChangeDistortionSpeedSettings(float duration, bool start)
    {
        float fromValue;
        float toValue;
        if (start)
        {
            fromValue = 0f;
            toValue = maxSpeed;
        }
        else
        {
            fromValue = maxSpeed;
            toValue = 0f;
        }

        return DOVirtual.Float(fromValue, toValue, duration, newVal =>
        {
            speed = newVal;
            heatMaterial.material.SetFloat("_Speed", speed);
        });
    }
}
