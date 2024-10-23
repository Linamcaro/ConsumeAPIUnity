using System.Collections;
using UnityEngine;

public class UIAnimationManager: MonoBehaviour 
{

    [SerializeField] private float fadeTime = 0.3f;
    [SerializeField] private float duration = 0.3f;

    public void SlideIn(Transform UIObject)
    {
        CanvasGroup canvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();

        UIAnimation.UIRightSlideIn(rectTransform , canvasGroup, fadeTime, duration);
    }

    public void SlideOut(Transform UIObject)
    {
        CanvasGroup canvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();
        UIAnimation.UIRightSlideOut(rectTransform, canvasGroup, fadeTime, duration);
    }

    public void ScaleUpFromPosition(Transform UIObject, Transform objectOrigin)
    {
        CanvasGroup canvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();

        rectTransform.position = objectOrigin.position;
        UIAnimation.UIScaleUp(rectTransform, canvasGroup, fadeTime, duration);
    }

    public void ScaleUp(Transform UIObject)
    {
        CanvasGroup canvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();

        UIAnimation.UIScaleUp(rectTransform, canvasGroup, fadeTime, duration);
    }

    public void ScaleOut(Transform UIObject)
    {
        CanvasGroup canvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform rectTransform = UIObject.GetComponent<RectTransform>();
        UIAnimation.UIScaleOut(rectTransform, canvasGroup, fadeTime, duration);
    }
}

