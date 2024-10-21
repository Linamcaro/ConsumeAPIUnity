using System;
using UnityEngine;
using DG.Tweening;

public static class UIAnimation
{
    /// <summary>
    /// slide in the object from the right side of the screen
    /// </summary>
    /// <param name="objectRectTransform"></param>
    /// <param name="canvasGroup"></param>
    /// <param name="fadeTime"></param>
    /// <param name="scaleDuration"></param>
    public static void UIRightSlideIn(RectTransform objectRectTransform, CanvasGroup canvasGroup, float fadeTime=1, float duration=1)
    {
        canvasGroup.alpha = 0f;

        objectRectTransform.anchoredPosition = new Vector2(1455f, 53f);

        objectRectTransform.DOAnchorPos(new Vector2(0f,53f),duration,false).SetEase(Ease.InOutCubic);
        canvasGroup.DOFade(1,fadeTime);
    }

    public static void UIRightSlideOut(RectTransform objectRectTransform, CanvasGroup canvasGroup, float fadeTime =1, float duration=1)
    {
        canvasGroup.alpha = 1f;

        objectRectTransform.anchoredPosition = new Vector2(0, 53f);
        objectRectTransform.DOAnchorPos(new Vector2(1455f,53f),duration,false).SetEase(Ease.OutCubic);
        canvasGroup.DOFade(0,fadeTime);
    }

    /// <summary>
    /// Scale object while it fade in to a certain position
    /// </summary>
    /// <param name="objectRectTransform"></param>
    /// <param name="canvasGroup"></param>
    /// <param name="fadeTime"></param>
    /// <param name="scaleDuration"></param>
    public static void UIScaleUp(RectTransform objectRectTransform, CanvasGroup canvasGroup, float fadeTime =1, float scaleDuration=1)
    {
        canvasGroup.alpha = 0f;

        objectRectTransform.DOAnchorPos(new Vector2(0f, 22f),scaleDuration,false).SetEase(Ease.InOutExpo);
        objectRectTransform.DOScale(Vector3.one, scaleDuration);
        canvasGroup.DOFade(1,fadeTime);
    }

    public static void UIScaleOut(RectTransform objectRectTransform, CanvasGroup canvasGroup, float fadeTime =1, float scaleDuration=1)
    {
        canvasGroup.alpha = 1f;

        objectRectTransform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.OutCubic);
        canvasGroup.DOFade(0,fadeTime);
    }


}
