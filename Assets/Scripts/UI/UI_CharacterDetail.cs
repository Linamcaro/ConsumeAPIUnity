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

   void Start()
   {
    
        //HideCharacterDetails();

   }

    public void SetCharacterDetails(Character character)
    {
        characterName.text = character.name;
        characterStatus.text = character.status;
        characterSpecie.text = character.species;
        characterLastLocation.text = character.location.name;
        characterFirstSeenLocation.text = character.origin.name;

        gameObject.SetActive(true);
    }

    public void HideCharacterDetails()
    {
        gameObject.SetActive(false);
    }
   
}
