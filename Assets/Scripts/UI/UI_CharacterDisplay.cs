using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Test.CharacterRepository;
using UnityEditor.PackageManager;
using System;


public class UI_CharacterDisplay : MonoBehaviour
{
    #region ============= UI Variables ===============
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;
   [SerializeField] private Transform characterPanel;
    //Pagination
   [SerializeField] private Transform pageItemTemplate;
   [SerializeField] private Transform paginationItemsContainer;

   //Character Detail Variables
   [SerializeField] private UI_CharacterDetail characterDetail;
   [SerializeField] private Transform characterDetailPanel;

   [SerializeField] private Transform errorPanel;
   [SerializeField] private Transform uiMenuOptions;

   //object pool
   private ObjectPool<Transform> characterItemPool;
   private ObjectPool<Transform> pageButtonItemPool;

   private CharacterRepository characterRepository;

   #endregion

    #region ============= Utility Variables ===============
    //the current page that user is viewing
    private int currentPage;
    private int totalPages;
    // Flag to check if pagination buttons have already been created
    private bool isPaginationCreated;

    #endregion

    public event Action<Transform> OnCharacterListRequest;
    public event Action<Transform> OnChangePage;
    public event Action<Transform, Transform> OnCharacterInfoRequest;
    public event Action<Transform> OnErrorTriggered;


    #region ============= Unity Lifecycle Methods ===============

    private void Awake() {
        characterItemTemplate.gameObject.SetActive(false);
        pageItemTemplate.gameObject.SetActive(false);
        characterPanel.gameObject.SetActive(false);

        //characterPanel.gameObject.SetActive(false);

        characterRepository = CharacterRepository.Instance;

        //event suscription
        characterRepository.OnWebRequestError += OnWebRequestError;
        characterRepository.OnCharacterRequest += OnCharacterRequest;

        // Initialize the object pool with an initial size of 10 (you can adjust the size as needed)
        characterItemPool = new ObjectPool<Transform>(characterItemTemplate, characterContainer, 2);
        pageButtonItemPool = new ObjectPool<Transform>(pageItemTemplate, paginationItemsContainer, 2);

        //variables set up
        currentPage = 0;
        isPaginationCreated = false;

    }

    private void Start()
    {
        //Load the first page 
       LoadPage(1);
    }

    #endregion

    #region ============= Event Handling Functions ===============

    private void OnCharacterRequest(CharacterResponse response)
    {

        CharacterResponse characterResponse = response;

        ClearCharacterItems();

        CreateCharacterITem(response);

        OnCharacterListRequest?.Invoke(characterPanel); 

        characterContainer.gameObject.SetActive(true);
        paginationItemsContainer.gameObject.SetActive(true);

         // Check if the page buttons are already created
        if (!isPaginationCreated)
        {
            CreatePageButtonITem(response);
            isPaginationCreated = true; // Mark pagination as created
        }

    }

   /// <summary>
   /// Show error Message to user
   /// </summary>
   /// <param name="errorMessage"></param>
   private void OnWebRequestError(string errorMessage)
   {  
      OnChangePage?.Invoke(characterPanel);

      paginationItemsContainer.gameObject.SetActive(false);
      uiMenuOptions.gameObject.SetActive(false);
      
      OnErrorTriggered?.Invoke(errorPanel);
      errorPanel.Find("errorText").GetComponent<TextMeshProUGUI>().text = $"Failed to retrieve data. Error {errorMessage}. Please try again";
      
      //errorPanel.gameObject.SetActive(true);
    
      Button tryAgainButton = errorPanel.Find("tryAgainButton").GetComponent<Button>();
      
      tryAgainButton.onClick.AddListener(()=>{
            //Call for the character details
            StartCoroutine(characterRepository.LoadCharacters(currentPage));
            //Hide the menu options buttons
            uiMenuOptions.gameObject.SetActive(true);
            //Hide the error panel
            errorPanel.gameObject.SetActive(false);
        });

        
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

    /// <summary>
    /// Spawn a page button item template
    /// </summary>
    private void CreatePageButtonITem(CharacterResponse characterResponse)
    {
        totalPages = characterResponse.info.pages;

        if(totalPages > 0)
        {
            for(int page = 1; page <= totalPages; page++)
            {
                int pageNumber = page;

                // Retrieve from the page button item pool the item template inside the container
                Transform pageItemTransform = pageButtonItemPool.GetObject(); 
                
                //set the page text number
                TextMeshProUGUI pageTextUI = pageItemTransform.Find("page").GetComponent<TextMeshProUGUI>();
                pageTextUI.text = pageNumber.ToString();
                
                pageItemTransform.gameObject.SetActive(true);
                
                //Set event for when a page number button is clicked
                Button pageNumberButton = pageItemTransform.GetComponent<Button>();
                pageNumberButton.onClick.AddListener(() => {

                    OnChangePage?.Invoke(characterPanel);
                    LoadPage(pageNumber);
                    });

            }
        }
    }
    #endregion

    #region ============= UI functionality functions ===============
    
    /// <summary>
    /// Function is called when a page button is pressed
    /// </summary>
    /// <param name="pageNumber"></param>
    public void LoadPage(int pageNumber)
    {
        if(currentPage == pageNumber) return;

        currentPage = pageNumber;

        StartCoroutine(characterRepository.LoadCharacters(currentPage));

    }

    /// <summary>
    /// Function when the next page button is called
    /// </summary>
    public void NextPage()
    {
        if(currentPage < totalPages)
        {
            int page = currentPage + 1;

            OnChangePage?.Invoke(characterPanel);

            LoadPage(page);
        }
    }

    /// <summary>
    /// Function when the previous page button is called
    /// </summary>
    public void PreviousPage()
    {
        
        if(currentPage > 1)
        {
            int page = currentPage -1;

            OnChangePage?.Invoke(characterPanel);

            LoadPage(page);  
        };
    }

    private void ShowCharacterInfo(Character characterInfo, Transform characterItemTransform)
    {
        characterDetail.SetCharacterDetails(characterInfo);

        OnCharacterInfoRequest?.Invoke(characterDetailPanel, characterItemTransform);

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
  
}



