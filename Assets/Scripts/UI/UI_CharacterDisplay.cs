using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Test.CharacterRepository;
using System;
using System.Collections;


public class UI_CharacterDisplay : MonoBehaviour
{
    #region ============= UI Variables ===============
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;
   [SerializeField] private Transform characterPanel;
   [SerializeField] private Transform uiMenuOptions;
   //Character Detail Variables
   [SerializeField] private UI_CharacterDetail characterDetail;
   [SerializeField] private Transform characterDetailPanel;

   private CharacterRepository characterRepository;
   //object pool
   private ObjectPool<Transform> characterItemPool;

   [SerializeField]private UIAnimationManager uiAnimationManager;
   #endregion

    private void Awake() 
    {
        characterItemTemplate.gameObject.SetActive(false);
        characterPanel.gameObject.SetActive(false);

        characterRepository = CharacterRepository.Instance;
        //event suscription
        characterRepository.OnCharacterRequest += OnCharacterRequest;
        // Initialize the object pool with an initial size of 10 (you can adjust the size as needed)
        characterItemPool = new ObjectPool<Transform>(characterItemTemplate, characterContainer, 2);
    }

    #region ============= Event Handling Functions ===============
    private void OnCharacterRequest(CharacterResponse response)
    {
        CharacterResponse characterResponse = response;

        ClearCharacterItems();
        CreateCharacterITem(response);

        uiMenuOptions.gameObject.SetActive(true);

        StartCoroutine(IESlideIn());

        characterContainer.gameObject.SetActive(true);
    }
    #endregion

    #region ============= UI creation functions ===============
    /// <summary>
    /// Create a Character button item
    /// </summary>
    private void CreateCharacterITem(CharacterResponse characterResponse)
    {
        if (characterResponse != null)
        {
            foreach (var character in characterResponse.results)
            {
                Character characterInfo = character;
                //Retrieve from the character item pool the item template inside the container
                Transform characterItemTransform = characterItemPool.GetObject(); 

                //set the character name
                characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = characterInfo.name;

                //Set up button OnClick functionality
                Button characterInformationButton = characterItemTransform.GetComponent<Button>();
                characterInformationButton.onClick.AddListener(() => ShowCharacterInfo(characterInfo, characterItemTransform )
                );
            }
        }
    }
    #endregion

    #region ============= UI functionality functions ===============
    private void ShowCharacterInfo(Character characterInfo, Transform characterItemTransform)
    {
        characterDetail.SetCharacterDetails(characterInfo);

        uiAnimationManager.ScaleUpFromPosition(characterDetailPanel, characterItemTransform);
        characterDetailPanel.gameObject.SetActive(true);

        gameObject.SetActive(false);
        uiMenuOptions.gameObject.SetActive(false); 
    }
    #endregion

    #region ============= Helper functions ===============
    /// <summary>
    /// return the character item to the pool
    /// </summary>
    private void ClearCharacterItems()
    {
        foreach (Transform child in characterContainer)
        {
            // Return the active objects back to the pool
            if (child.gameObject.activeSelf)
            {
                characterItemPool.ReturnObject(child);
            }
        }
    }
    #endregion

    IEnumerator IESlideIn()
    {
        yield return new WaitForSeconds(0.5f);
        uiAnimationManager.SlideIn(characterPanel);
        characterPanel.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        characterRepository.OnCharacterRequest -= OnCharacterRequest;
    }
}



