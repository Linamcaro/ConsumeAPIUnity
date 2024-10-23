using System.Collections;
using Test.CharacterRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ErrorMessage : MonoBehaviour
{
    [SerializeField] private Transform errorPanel;
    [SerializeField] private Transform uiMenuOptions;
    [SerializeField] private UIAnimationManager uiAnimationManager;

    void Awake()
    {

        CharacterRepository.Instance.OnWebRequestError += OnWebRequestError;
    }

   /// <summary>
   /// Show error Message to user
   /// </summary>
   /// <param name="errorMessage"></param>
   private void OnWebRequestError(string errorMessage, int page)
   {  

      uiMenuOptions.gameObject.SetActive(false);

      errorPanel.Find("errorText").GetComponent<TextMeshProUGUI>().text = $"Oh Oh there is a problem. {errorMessage}. Please try again";

      StartCoroutine(ScaleUpAnimation());

      Button tryAgainButton = errorPanel.Find("tryAgainButton").GetComponent<Button>();
      
      tryAgainButton.onClick.AddListener(() => TryAgain(page));
   }

   private void TryAgain(int page)
   {
      uiAnimationManager.ScaleOut(errorPanel);
      StartCoroutine(HideGameObject());
      //Call for the character details
      StartCoroutine( CharacterRepository.Instance.LoadCharacters(page));
 
   }

   public void OnButtonClose()
   {
      uiAnimationManager.ScaleOut(errorPanel);
   }

   IEnumerator HideGameObject()
   {
      yield return new WaitForSeconds(0.4f);
      errorPanel.gameObject.SetActive(false);
   }

   IEnumerator ScaleUpAnimation()
   {
      yield return new WaitForSeconds(0.5f);
      uiAnimationManager.ScaleUp(errorPanel);
      errorPanel.gameObject.SetActive(true);
   }

}
