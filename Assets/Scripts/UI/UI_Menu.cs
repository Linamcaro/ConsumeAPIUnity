using System.Collections;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private UIAnimationManager uiAnimationManager;
    [SerializeField] private Transform optionsPanel;
    [SerializeField] private Transform homeButton;

    public void OpenMenu()
    {
        uiAnimationManager.ScaleUpFromPosition(optionsPanel, homeButton);
        optionsPanel.gameObject.SetActive(true);
    }   

    public void CloseMenu()
    {
       uiAnimationManager.ScaleOut(optionsPanel);
       StartCoroutine(HideGameObject());
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    
    IEnumerator HideGameObject()
    {
        yield return new WaitForSeconds(0.12f);
        optionsPanel.gameObject.SetActive(false);
    }
 
    
}
