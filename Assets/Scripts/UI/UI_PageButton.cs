using System;
using System.Collections;
using System.Collections.Generic;
using Test.CharacterRepository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PageButton : MonoBehaviour
{
    #region =========== UI functions ==========
    [SerializeField] private Transform pageItemTemplate;
    [SerializeField] private Transform paginationItemsContainer;
    [SerializeField] private Transform characterPanel;
    [SerializeField] private UIAnimationManager uiAnimationManager;
    [SerializeField] private Scrollbar scrollbar;
    #endregion 

    #region =========== Helper functions ==========
    //the current page that user is viewing
    private int currentPage;
    private int totalPages;
    // Flag to check if pagination buttons have already been created
    private bool isPaginationCreated;
    private List<Button> pageButtonsList = new List<Button>();

    #endregion
    
    private ObjectPool<Transform> pageButtonItemPool;
    private CharacterRepository characterRepository;

    void Awake()
    {
        pageItemTemplate.gameObject.SetActive(false);

        characterRepository = CharacterRepository.Instance;
        //event suscription
        characterRepository.OnCharacterRequest += OnCharacterRequest;
        characterRepository.OnWebRequestError += OnWebRequestError;

        pageButtonItemPool = new ObjectPool<Transform>(pageItemTemplate, paginationItemsContainer, 2);

        //variables set up
        currentPage = 1;
        isPaginationCreated = false;
    }

    private void Start() {
        StartCoroutine(characterRepository.LoadCharacters(currentPage));
    }

    #region ============= Network functions ===============
    private void OnWebRequestError(string arg1, int arg2)
    {
        StartCoroutine(IESlideOutAnimation());
        
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


                Button pageNumberButton = pageItemTransform.GetComponent<Button>();
                pageButtonsList.Add(pageNumberButton);
                //Set event for when a page number button is clicked
                pageNumberButton.onClick.AddListener(() => {
                    
                    LoadPage(pageNumber);
                });

                pageItemTransform.gameObject.SetActive(true);

                if(page == 1)
                {
                    ButtonSelected(pageNumberButton);
                }
            }
        }
    }


    #region ============= Pagination functions ===============
    /// <summary>
    /// Function is called when a page button is pressed
    /// </summary>
    /// <param name="pageNumber"></param>
    public void LoadPage(int pageNumber)
    {
        if(currentPage == pageNumber || pageNumber == 0) return;

        if(pageButtonsList[currentPage - 1] != null)
        {
            ButtonDeselected(pageButtonsList[currentPage - 1]);

        };

        currentPage = pageNumber;

        StartCoroutine(IESlideOutAnimation());
        StartCoroutine(characterRepository.LoadCharacters(currentPage));

        MoveScrollbar();  
    }

    /// <summary>
    /// Function when the next page button is called
    /// </summary>
    public void NextPage()
    {
        if(currentPage < totalPages)
        {
            int page = currentPage + 1;

            ButtonSelected(pageButtonsList[page-1]);

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

            ButtonSelected(pageButtonsList[page-1]);

            LoadPage(page);  
        };
    }
    #endregion

    #region ===== Visual Update UI functions =====
    private void MoveScrollbar()
    {
        // Set scrollbar value based on current page
        float newScrollbarValue = (float)(currentPage - 1) / (totalPages - 1);
        scrollbar.value = newScrollbarValue;
    }

    private void ButtonSelected(Button button)
    {
       ColorBlock colors = button.colors;
       colors.normalColor = colors.highlightedColor;  
       button.colors = colors;
    }

    private void ButtonDeselected(Button button) 
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.gray;
        button.colors = colors;
    }
    #endregion

    
    IEnumerator IESlideOutAnimation()
    {
        uiAnimationManager.SlideOut(characterPanel);
        yield return new WaitForSeconds(0.5f);
        characterPanel.gameObject.SetActive(false);
    }

    private void OnDestroy() 
    {
        characterRepository.OnCharacterRequest -= OnCharacterRequest;
        characterRepository.OnWebRequestError -= OnWebRequestError;
    }

    

}
