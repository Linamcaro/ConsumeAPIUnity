using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UICharacterDetailAnimation : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float scaleDuration = 0.5f;
    private UI_CharacterDetail characterDetail;
    

    private void Awake()
    {
        
        characterDetail = GetComponent<UI_CharacterDetail>();

        characterDetail.OnCharacterInfoClose += UIAnimation_OnCharacterInfoClose;
    }


    private void UIAnimation_OnCharacterInfoClose(object sender, EventArgs e)
    {
        CanvasGroup UICanvasGroup = GetComponent<CanvasGroup>();
        RectTransform UIRectTransform = GetComponent<RectTransform>();

        
        UIAnimation.UIScaleOut(UIRectTransform , UICanvasGroup, fadeTime, scaleDuration);

        StartCoroutine(HideGameObject(UIRectTransform));
    }

    IEnumerator HideGameObject(RectTransform rectTransform)
    {
        yield return new WaitForSeconds(0.12f);
        rectTransform.gameObject.SetActive(false);
    }

}
