using System;
using System.Collections;
using UnityEngine;

public class UICharacterListAnimation : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float duration = 0.5f;
    private UI_CharacterDisplay characterDisplay;
    private UI_PageButton pageButton;
    

    private void Awake() 
    {
        characterDisplay = GetComponent<UI_CharacterDisplay>();
        pageButton = GetComponent<UI_PageButton> ();
        
        characterDisplay.OnCharacterListRequest += UIAnimation_OnCharacterListRequest;
        characterDisplay.OnCharacterInfoRequest += UIAnimation_OnCharacterInfoRequest;

        pageButton.OnChangePage += UIAnimation_OnChangePage;  
    }

    #region ========= Event Functions ==========
    private void UIAnimation_OnCharacterListRequest(Transform UIObject)
    {
        Debug.Log("OnCharacterListRequest called");

        CanvasGroup UICanvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform UIRectTransform = UIObject.GetComponent<RectTransform>();

        StartCoroutine(SlideInUI(UIRectTransform, UICanvasGroup));
    }
    
    private void UIAnimation_OnCharacterInfoRequest(Transform UIObject, Transform characterItemTransform )
    {
        Debug.Log("OnCharacterinfotRequest called");

        UIObject.localScale = Vector2.zero;

        CanvasGroup UICanvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform UIRectTransform = UIObject.GetComponent<RectTransform>();

        ScaleUpUI(UIRectTransform, UICanvasGroup, characterItemTransform);
    }

    private void UIAnimation_OnChangePage(Transform UIObject)
    {
        CanvasGroup UICanvasGroup = UIObject.GetComponent<CanvasGroup>();
        RectTransform UIRectTransform = UIObject.GetComponent<RectTransform>();

        SlideOutUI(UIRectTransform, UICanvasGroup);
    }

   
    #endregion

    #region ========= Animation Functions ==========
    private void ScaleUpUI(RectTransform rectTransform, CanvasGroup canvasGroup, Transform characterItemTransform) 
    {
        rectTransform.position = characterItemTransform.position;
        UIAnimation.UIScaleUp(rectTransform , canvasGroup, fadeTime, duration);
        rectTransform.gameObject.SetActive(true);
    }

    private void SlideOutUI(RectTransform panelRectTransform, CanvasGroup UICanvasGroup)
    {
        UIAnimation.UIRightSlideOut(panelRectTransform, UICanvasGroup, fadeTime, duration);
        StartCoroutine(HideGameObject(panelRectTransform));
    }
    #endregion


    #region ========= Coroutines Functions ==========
    IEnumerator SlideInUI(RectTransform rectTransform, CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(0.7f);
        rectTransform.gameObject.SetActive(true);
        UIAnimation.UIRightSlideIn(rectTransform , canvasGroup, fadeTime, duration);
    }

    IEnumerator HideGameObject(RectTransform rectTransform)
    {
        yield return new WaitForSeconds(0.7f);
        rectTransform.gameObject.SetActive(false);
    }
    #endregion
}
