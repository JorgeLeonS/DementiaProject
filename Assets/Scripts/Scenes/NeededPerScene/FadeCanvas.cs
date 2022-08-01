using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class FadeCanvas : MonoBehaviour
{
    private static CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public static Tween FadeIn(float duration = 1f)
    {
        return DOVirtual.Float(canvasGroup.alpha, 1f, duration, newVal => {
            canvasGroup.alpha = newVal;
        });
    }

    public static Tween FadeOut(float duration = 1f)
    {
        return DOVirtual.Float(canvasGroup.alpha, 0f, duration, newVal => {
            canvasGroup.alpha = newVal;
        });
    }

    public static void FadeInOut()
    {
        FadeIn().OnComplete(() => FadeOut());
    }

    public static IEnumerator FadeInOutWithAction(Action action)
    {
        FadeIn().OnComplete(() =>
        {
            action();
            FadeOut();
        });
        //TODO Change for a propoper Wait
        yield return new WaitForSeconds(3f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    FadeIn();
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    FadeOut();
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    FadeInOut();
        //}
    }
}
