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
    public Volume sceneVolume;
    private static Bloom bloom;

    public GameObject blinds;
    private static Animator blindsAnimator;

    private int interactionCounter;

    // Start is called before the first frame update
    void Start()
    {
        SceneEvents.current.sceneAction += PerformSceneAction;

        Lights_Manager.ChangeAmbientLightIntensity(0.1f, 0.1f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        try
        {
            if(sceneVolume == null)
            {
                sceneVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>();
            }
            if(blinds == null)
            {
                blinds = GameObject.Find("Blinds");
            }
            blindsAnimator = blinds.GetComponent<Animator>();

        }
        catch (System.Exception e)
        {
            Debug.LogError($"PostProcessVolume / BlindsAnimator could not be found on the scene! \n" +
                $"Is the naming correct?\n {e}");
        }
        sceneVolume.profile.TryGet<Bloom>(out bloom);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        SceneEvents.current.sceneAction -= PerformSceneAction;
    }

    public IEnumerator PerformSceneAction()
    {
        switch (interactionCounter)
        {
            case 0:
                yield return OpenBlinds();
                break;
            default:
                yield return new WaitForSeconds(3f);
                Debug.Log("No more scene actions!");
                break;
        }
        interactionCounter++;
        SceneEvents.current.CompletedInteraction();
    }

    #region Scene specific methods
    /// <summary>
    /// When the 
    /// </summary>
    public static IEnumerator OpenBlinds_VisualEffect()
    {
        var hasSequenceCompleted = false;
        Sequence blindsOpenSequence = DOTween.Sequence();
        Lights_Manager.ChangeAmbientLightIntensity(1, 0.5f);
        Lights_Manager.ChangeEnvironmentReflectionsIntensity(0.2f, 0.1f);
        blindsOpenSequence.Append(PostProcess_Manager.ChangeBloom_Intensity(bloom, 50, 3f));
        
        blindsOpenSequence.AppendInterval(1.5f);
        yield return new WaitForSeconds(1.5f);
        
        PostProcess_Manager.ChangeBloom_Intensity(bloom, 0.1f, 4f);
        blindsOpenSequence.OnComplete(() => {
            hasSequenceCompleted = true;
        });
        yield return new WaitUntil(() => hasSequenceCompleted);
    }

    public static IEnumerator OpenBlinds_Animation()
    {
        blindsAnimator.SetTrigger("PullBlinds");
        yield return new WaitForSeconds(0.7f);
        print("Animation finished");
    }

    public static IEnumerator OpenBlinds() 
    {
        yield return OpenBlinds_Animation();
        yield return OpenBlinds_VisualEffect();
    }
    #endregion
}
