using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    private static CanvasGroup canvasGroup;
    public static Sequence FadeInOutSequence;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    public static Tween FadeIn()
    {
        return DOVirtual.Float(canvasGroup.alpha, 1f, 1f, newVal => {
            canvasGroup.alpha = newVal;
        });
    }

    public static Tween FadeOut()
    {
        return DOVirtual.Float(canvasGroup.alpha, 0f, 1f, newVal => {
            canvasGroup.alpha = newVal;
        });
    }

    public static void FadeInOut()
    {
        FadeIn().OnComplete(() => FadeOut());
        //OnComplete(FadeIn
        //FadeInOutSequence = DOTween.Sequence();
        //FadeInOutSequence.Append(FadeIn()).SetEase(Ease.);
        //FadeInOutSequence.Append(FadeOut());
    }

    // Start is called before the first frame update
    void Start()
    {
        //FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FadeOut();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FadeInOut();
        }
    }
}
