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
    private static ShadowsMidtonesHighlights smh;

    public GameObject blinds;
    private static Animator blindsAnimator;

    public GameObject doorArrow;
    public GameObject doorText;
    private static TextRevealer TRDoorText;

    private int interactionCounter;

    // Start is called before the first frame update
    void Start()
    {
        SceneEvents.current.sceneAction += PerformSceneAction;

        doorArrow.SetActive(false);

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
            TRDoorText = doorText.transform.GetChild(0).GetComponent<TextRevealer>();

        }
        catch (System.Exception e)
        {
            Debug.LogError($"PostProcessVolume / BlindsAnimator could not be found on the scene! \n" +
                $"Is the naming correct?\n {e}");
        }
        sceneVolume.profile.TryGet<Bloom>(out bloom);
        sceneVolume.profile.TryGet<ShadowsMidtonesHighlights>(out smh);
        smh.active = true;

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
            case 1:
                ActivateDoorText();
                break;
            default:
                yield return new WaitForSeconds(3f);
                Debug.Log("No more scene actions!");
                break;
        }
        interactionCounter++;
        //SceneEvents.current.CompletedAction();
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
        blindsOpenSequence.Append(PostProcess_Manager.ChangeBloom_Intensity(bloom, 200, 3.5f));
        
        
        blindsOpenSequence.AppendInterval(2f);
        yield return new WaitForSeconds(2f);

        // TODO Change these to PPManager
        smh.active = false;
        // TODO Change these to PPManager
        //smh.shadows.value = new Vector4(0.32f, 0.32f, 0.32f, 0);
        //smh.midtones.value = new Vector4(0.57f, 0.57f, 0.57f, 0);

        PostProcess_Manager.ChangeBloom_Intensity(bloom, 0.1f, 4f);
        blindsOpenSequence.OnComplete(() => {
            hasSequenceCompleted = true;
        });
        yield return new WaitUntil(() => hasSequenceCompleted);
    }

    public void ActivateDoorText()
    {
        TRDoorText.Reveal();
        doorArrow.SetActive(true);
    }

    public static IEnumerator OpenBlinds_Animation()
    {
        blindsAnimator.SetTrigger("PullBlinds");
        yield return new WaitForSeconds(0.7f);
    }

    public static IEnumerator OpenBlinds() 
    {
        yield return OpenBlinds_Animation();
        yield return OpenBlinds_VisualEffect();
    }
    #endregion
}
