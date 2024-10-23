using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_CharacterDetail : MonoBehaviour
{
    #region ============= UI Variables ===============
   [SerializeField] private TextMeshProUGUI characterName;
   [SerializeField] private TextMeshProUGUI characterStatus;
   [SerializeField] private TextMeshProUGUI characterSpecie;
   [SerializeField] private TextMeshProUGUI characterLastLocation;
   [SerializeField] private TextMeshProUGUI characterFirstSeenLocation;
   #endregion

   [SerializeField] private UIAnimationManager uiAnimationManager;

    public void SetCharacterDetails(Character character)
    {
        characterName.text = character.name;
        characterStatus.text = character.status;
        characterSpecie.text = character.species;
        characterLastLocation.text = character.location.name;

    }

    public void HideCharacterDetails()
    {
       uiAnimationManager.ScaleOut(gameObject.transform);
       StartCoroutine(HideGameObject());
    }

     IEnumerator HideGameObject()
    {
        yield return new WaitForSeconds(0.12f);
        gameObject.SetActive(false);
    }
 
}
