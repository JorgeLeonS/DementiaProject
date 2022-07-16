using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToothbrushEffect : MonoBehaviour
{
    //[SerializeField] List<Renderer> toothbrushes = new List<Renderer>();
    [SerializeField] Renderer toothbrush;
    [Range(0f, 5f)]
    [SerializeField] float maxToothbrushVisibility = 0.5f;
    [Range(0.01f, 3f)]
    [SerializeField] float timeToTransitionVisibility = 1f;

    float toothbrushVisibility;

    private void Start()
    {
        InvisibleObject();
    }

    private void InvisibleObject()
    {
        foreach(Material material in toothbrush.materials)
        {
            material.SetFloat("_Power", 0f);
        }
    }

    public void StartToothbrushEffect()
    {
        MakeToothbrushVisible(timeToTransitionVisibility, true);
    }

    public void ReverseToothbrushEffect()
    {
        MakeToothbrushVisible(timeToTransitionVisibility, false);
    }

    private Tween MakeToothbrushVisible(float duration, bool makeVisible)
    {
        float fromValue, toValue;
        if (makeVisible)
        {
            fromValue = 0f;
            toValue = maxToothbrushVisibility;
        }
        else
        {
            fromValue = maxToothbrushVisibility;
            toValue = 0f;
        }
        
        return DOVirtual.Float(fromValue, toValue, duration, newVal => {
            toothbrushVisibility = newVal;
            foreach(Material material in toothbrush.materials)
            {
                material.SetFloat("_Power", toothbrushVisibility);
            }
        });
    }
}
