using System;
using System.Collections;
using UnityEngine;

public class UIErrorMessageAnimation : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float duration = 0.5f;

    private UI_ErrorMessage errorMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        errorMessage = GetComponent<UI_ErrorMessage>();

        errorMessage.OnErrorTriggered += UIAnimation_OnErrorTriggered;
        errorMessage.OnButtonPressed += UIAnimation_OnTryAgainPressed;
        
    }


    private void UIAnimation_OnErrorTriggered(Transform UIObject)
    {
        CanvasGroup UICanvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform UIRectTransform = UIObject.GetComponent<RectTransform>();

        UIAnimation.UIScaleUp(UIRectTransform , UICanvasGroup, fadeTime, duration);
        UIRectTransform.gameObject.SetActive(true);
    }

    private void UIAnimation_OnTryAgainPressed(Transform UIObject)
    {
       CanvasGroup UICanvasGroup = UIObject.GetComponent<CanvasGroup>();
       RectTransform UIRectTransform = UIObject.GetComponent<RectTransform>();
       
       UIAnimation.UIScaleOut(UIRectTransform , UICanvasGroup, fadeTime, duration);
       //UIRectTransform.gameObject.SetActive(false);
       HideGameObject(UIRectTransform);
    }


    
    IEnumerator HideGameObject(RectTransform rectTransform)
    {
        yield return new WaitForSeconds(0.5f);
        rectTransform.gameObject.SetActive(false);
    }
}
