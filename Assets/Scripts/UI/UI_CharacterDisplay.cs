using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Test.CharacterRepository;
using UnityEditor.PackageManager;


public class UI_CharacterDisplay : MonoBehaviour
{
    #region ============= UI Variables ===============
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;
    //Pagination
   [SerializeField] private Transform pageContainer;
   [SerializeField] private Transform pageItemTemplate;
   [SerializeField] private Transform paginationItemsContainer;
   [SerializeField] private Transform errorPanel;
   [SerializeField] private Transform uiMenuOptions;
   //character items object pool
   private ObjectPool<Transform> characterItemPool;

   [SerializeField] private UI_CharacterDetail characterDetail;

   private CharacterRepository characterRepository;

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
        uiMenuOptions.gameObject.SetActive(false);

        characterRepository = CharacterRepository.Instance;

        characterRepository.OnWebRequestError += OnWebRequestError;
        characterRepository.OnCharacterRequest += OnCharacterRequest;

        // Initialize the object pool with an initial size of 10 (you can adjust the size as needed)
        characterItemPool = new ObjectPool<Transform>(characterItemTemplate, characterContainer, 1);

        currentPage = 1;

        isPaginationCreated = false;
    }

    private void Start()
    {
        //Load the first page 
        StartCoroutine(characterRepository.LoadCharacters(currentPage));
    }

    #endregion

    #region ============= Event Handling Functions ===============

    private void OnCharacterRequest(CharacterResponse response)
    {

        CharacterResponse characterResponse = response;

        ClearCharacterItems();

        CreateCharacterITem(response);

         // Check if the page buttons are already created
        if (!isPaginationCreated)
        {
            CreatePageButtonITem(response);
            isPaginationCreated = true; // Mark pagination as created
        }
        
        characterContainer.gameObject.SetActive(true);
        paginationItemsContainer.gameObject.SetActive(true);
    }

   /// <summary>
   /// Show error Message to user
   /// </summary>
   /// <param name="errorMessage"></param>
   private void OnWebRequestError(string errorMessage)
   {
      characterContainer.gameObject.SetActive(false);
      paginationItemsContainer.gameObject.SetActive(false);
      uiMenuOptions.gameObject.SetActive(false);

      errorPanel.Find("errorText").GetComponent<TextMeshProUGUI>().text = $"Failed to retrieve data. Error {errorMessage}. Please try again";
      errorPanel.gameObject.SetActive(true);
    

      Button tryAgainButton = errorPanel.Find("tryAgainButton").GetComponent<Button>();
      
      tryAgainButton.onClick.AddListener(()=>{
            StartCoroutine(characterRepository.LoadCharacters(currentPage));
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
                // Duplicate or retrieve from pool the item template inside the container
                Transform characterItemTransform = characterItemPool.GetObject();  // Retrieve from object pool (assuming you're using the pool here)

                //set the character name
                characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = character.name;

                Character characterInfo = character;
                Button characterInformationButton = characterItemTransform.GetComponent<Button>();

                characterInformationButton.onClick.AddListener(() => {

                    characterDetail.SetCharacterDetails(characterInfo);
                    gameObject.SetActive(false);
                    uiMenuOptions.gameObject.SetActive(uiMenuOptions);

                });
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
    
    /// <summary>
    /// Function is called when a page button is pressed
    /// </summary>
    /// <param name="pageNumber"></param>
    public void LoadSpecificPage(int pageNumber)
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
            currentPage += 1;
            ClearCharacterItems();
            StartCoroutine(characterRepository.LoadCharacters(currentPage));
        }
    }

    /// <summary>
    /// Function when the previous page button is called
    /// </summary>
    public void PreviousPage()
    {
        if(currentPage > 0)
        {
            currentPage -= 1;
            ClearCharacterItems();
            StartCoroutine(characterRepository.LoadCharacters(currentPage));
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



