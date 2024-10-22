using System;
using Test.CharacterRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ErrorMessage : MonoBehaviour
{
    [SerializeField] private Transform errorPanel;
    [SerializeField] private Transform uiMenuOptions;
    public event Action<Transform> OnErrorTriggered;
    public event Action<Transform> OnButtonPressed;

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
      
      OnErrorTriggered?.Invoke(errorPanel);

      uiMenuOptions.gameObject.SetActive(false);

      errorPanel.Find("errorText").GetComponent<TextMeshProUGUI>().text = $"Failed to retrieve data. Error {errorMessage}. Please try again";

      Button tryAgainButton = errorPanel.Find("tryAgainButton").GetComponent<Button>();
      
      tryAgainButton.onClick.AddListener(() => TryAgain(page));
   }

   private void TryAgain(int page)
   {
        OnButtonPressed?.Invoke(errorPanel);

        //Call for the character details
        StartCoroutine( CharacterRepository.Instance.LoadCharacters(page));
            
        //Hide the error panel
        errorPanel.gameObject.SetActive(false);
   }

   public void OnButtonClose()
   {
     OnButtonPressed?.Invoke(errorPanel);
   }

}
