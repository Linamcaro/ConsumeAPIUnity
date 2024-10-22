using System;
using Test.CharacterRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PageButton : MonoBehaviour
{
    [SerializeField] private Transform pageItemTemplate;
    [SerializeField] private Transform paginationItemsContainer;
    [SerializeField] private Transform characterPanel;

    private ObjectPool<Transform> pageButtonItemPool;
    private CharacterRepository characterRepository;

    #region ============= Utility Variables ===============
    //the current page that user is viewing
    private int currentPage;
    private int totalPages;
    // Flag to check if pagination buttons have already been created
    private bool isPaginationCreated;

    #endregion

    public event Action<Transform> OnChangePage;
    
    void Awake()
    {
        pageItemTemplate.gameObject.SetActive(false);

        characterRepository = CharacterRepository.Instance;

        //event suscription
        characterRepository.OnCharacterRequest += OnCharacterRequest;
        characterRepository.OnWebRequestError += OnWebRequestError;

        pageButtonItemPool = new ObjectPool<Transform>(pageItemTemplate, paginationItemsContainer, 2);

        //variables set up
        currentPage = 0;
        isPaginationCreated = false;
    }

    private void Start() {
        LoadPage(1);
    }

    #region ============= Network functions ===============
    private void OnWebRequestError(string arg1, int arg2)
    {
        OnChangePage?.Invoke(characterPanel);
        
        paginationItemsContainer.gameObject.SetActive(false);
    }

    private void OnCharacterRequest(CharacterResponse response)
    {
        CharacterResponse characterResponse = response;

        paginationItemsContainer.gameObject.SetActive(true);

         // Check if the page buttons are already created
        if (!isPaginationCreated)
        {
            CreatePageButtonITem(response);
            isPaginationCreated = true; 
        }

    }
    #endregion

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
                    
                    LoadPage(pageNumber);
                    });

            }
        }
    }

    #region ============= Button functions ===============
    /// <summary>
    /// Function is called when a page button is pressed
    /// </summary>
    /// <param name="pageNumber"></param>
    public void LoadPage(int pageNumber)
    {
        if(currentPage == pageNumber) return;

        currentPage = pageNumber;

        OnChangePage?.Invoke(characterPanel);

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

            LoadPage(page);  
        };
    }
    #endregion
}
