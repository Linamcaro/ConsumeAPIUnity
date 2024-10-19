using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Test.ApiService;
using System.Collections;
using System;


public class UI_CharacterDisplay : MonoBehaviour
{
    #region ============= UI Variables ===============
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;
    
    //Pagination
   [SerializeField] private Transform pageContainer;
   [SerializeField] private Transform pageItemTemplate;

   [SerializeField] private Transform errorPanel;
   //character items object pool
   private ObjectPool<Transform> characterItemPool;

   [SerializeField] private UI_CharacterDetail characterDetail;

   #endregion

   #region ============= Network Variables ===============
   // Network variable
   private ApiRequest apiRequest;
   private CharacterResponse response;

   #endregion

    #region ============= Utility Variables ===============
    //the current page that user is viewing
    private int currentPage;
    private int totalPages;
    // Flag to check if pagination buttons have already been created
    private bool isPaginationCreated;

    #endregion

    #region ============= Unity Lifecycle Methods ===============
    private void Awake() {

        characterItemTemplate.gameObject.SetActive(false);
        pageItemTemplate.gameObject.SetActive(false);
        errorPanel.gameObject.SetActive(false);

        //Create instance of ApiRequest and subscribe to the OnWebRequest event
        apiRequest = new ApiRequest();
        apiRequest.OnWebRequestError += OnWebRequestError;

        // Initialize the object pool with an initial size of 10 (you can adjust the size as needed)
        characterItemPool = new ObjectPool<Transform>(characterItemTemplate, characterContainer, 1);

        currentPage = 1;

        isPaginationCreated = false;
    }

    private void Start()
    {
        StartCoroutine(LoadCharacters(currentPage));
        
    }
    #endregion
 
    #region ============= Network Functions ===============

   /// <summary>
   /// Spawn a character item template
   /// </summary>
   /// <param name="page"></param>
   private IEnumerator LoadCharacters(int page)
   {    
        // Clear the existing character items before loading new ones
        ClearCharacterItems();

        //Format the URL to request with the page number requested
        string formatedUrl = string.Format(Constants.apiUrl, page);
        var requestData = apiRequest.GetCharacter(formatedUrl);

        yield return requestData.SendWebRequest();

        response = JsonUtility.FromJson<CharacterResponse>(requestData.downloadHandler.text);

        CreateCharacterITem();

         // Check if the page buttons are already created
        if (!isPaginationCreated)
        {
            CreatePageButtonITem();
            isPaginationCreated = true; // Mark pagination as created
        }
   }

   /// <summary>
   /// Show error Message to user
   /// </summary>
   /// <param name="errorMessage"></param>
   private void OnWebRequestError(string errorMessage)
   {
      errorPanel.Find("errorText").GetComponent<TextMeshProUGUI>().text = "Information failed error: " + errorMessage;
      errorPanel.gameObject.SetActive(true);
   }
    #endregion

    #region ============= UI creation functions ===============
    /// <summary>
    /// Create a Character button item
    /// </summary>
    private void CreateCharacterITem()
    {

        if (response != null)
        {
            foreach (var character in response.results)
            {
                // Duplicate or retrieve from pool the item template inside the container
                Transform characterItemTransform = characterItemPool.GetObject();  // Retrieve from object pool (assuming you're using the pool here)

                //set the character name
                characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = character.name;

                Character characterInfo = character;
                Button characterInformationButton = characterItemTransform.GetComponent<Button>();

                characterInformationButton.onClick.AddListener(() => {

                    characterDetail.SetCharacterDetails(characterInfo);
                    gameObject.SetActive(false);

                });
            }
        }
    }

    /// <summary>
    /// Spawn a page button item template
    /// </summary>
    private void CreatePageButtonITem()
    {
        totalPages = response.info.pages;

        if(totalPages > 0)
        {
            for(int page = 1; page <= totalPages; page++)
            {
                //Duplicate the item template inside the container
                Transform pageItemTransform = Instantiate(pageItemTemplate, pageContainer);
                RectTransform pageItemRectTransform = pageItemTransform.GetComponent<RectTransform>(); 
                
                //set the page text number
                TextMeshProUGUI pageTextUI = pageItemTransform.Find("page").GetComponent<TextMeshProUGUI>();
                pageTextUI.text = page.ToString();
                
                pageItemTransform.gameObject.SetActive(true);
                
                //Set event for when a page number button is clicked
                int pageNumber = int.Parse(pageTextUI.text);
                Button pageNumberButton = pageItemTransform.GetComponent<Button>();
                pageNumberButton.onClick.AddListener(() => LoadSpecificPage(pageNumber));
            }
        }
    }
    #endregion

    #region ============= UI functionality functions ===============
   public void LoadSpecificPage(int pageNumber)
   {
    if(currentPage == pageNumber) return;

     currentPage = pageNumber;
     StartCoroutine(LoadCharacters(currentPage));
     
    }

    public void NextPage()
    {
        if(currentPage < totalPages)
        {
            currentPage += 1;
            ClearCharacterItems();
            StartCoroutine(LoadCharacters(currentPage));
        }
    }

    public void PreviousPage()
    {
        if(currentPage > 0)
        {
            currentPage -= 1;
            ClearCharacterItems();
            StartCoroutine(LoadCharacters(currentPage));
        };
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
  
}



