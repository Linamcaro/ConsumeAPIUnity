using System;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using Prueba.ApiService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_CharacterDisplay : MonoBehaviour
{
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;

    //Pagination
   [SerializeField] private Transform pageContainer;
   [SerializeField] private Transform pageItemTemplate;

   //character items object pool
    private ObjectPool<Transform> characterItemPool;

   // Network variable
   private ApiRequest apiRequest;
   private CharacterResponse response;

   //the current page that user is viewing
   private int currentPage = 1;


   private void Awake() {

        characterItemTemplate.gameObject.SetActive(false);
        pageItemTemplate.gameObject.SetActive(false);

        apiRequest = new ApiRequest();

        // Initialize the object pool with an initial size of 10 (you can adjust the size as needed)
        characterItemPool = new ObjectPool<Transform>(characterItemTemplate, characterContainer, 1);

   }

    private async void Start()
    {
        await LoadCharacters(currentPage);
        CreatePageButtonITem();
    }
 

   /// <summary>
   /// Spawn a character item template
   /// </summary>
   /// <param name="intemName"></param>
   public async Task LoadCharacters(int page)
   {    
        // Clear the existing character items before loading new ones
        ClearCharacterItems();

        response = await apiRequest.GetCharacter(page);

        CreateCharacterITem();
   }

    /// <summary>
    /// Create a Character button item
    /// </summary>
    private void CreateCharacterITem()
   {

    if (response != null)
        {
            foreach (var character in response.results)
            {

                /*//Duplicate the item template inside the container
                Transform characterItemTransform = Instantiate(characterItemTemplate, characterContainer);
                RectTransform characterItemRectTransform = characterItemTransform.GetComponent<RectTransform>(); */

                // Duplicate or retrieve from pool the item template inside the container
                 Transform characterItemTransform = characterItemPool.GetObject();  // Retrieve from object pool (assuming you're using the pool here)
                 
                 
                    
                //set the character name
                characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = character.name;

                //characterItemTransform.gameObject.SetActive(true);

                //Button button = characterItemTransform.GetComponent<Button>();
            }

        }

   }


      /// <summary>
   /// Spawn a page button item template
   /// </summary>
   /// <param name="intemName"></param>
   private void CreatePageButtonITem()
   {

        int totalPages = response.info.pages;

        if(totalPages > 0)
        {

            for(int page = 1; page <= totalPages; page++)
            {
                
                //Duplicate the item template inside the container
                Transform pageItemTransform = Instantiate(pageItemTemplate, pageContainer);
                RectTransform pageItemRectTransform = pageItemTransform.GetComponent<RectTransform>(); 
                
                //set the text
                TextMeshProUGUI pageTextUI = pageItemTransform.Find("page").GetComponent<TextMeshProUGUI>();
                pageTextUI.text = page.ToString();
                

                pageItemTransform.gameObject.SetActive(true);
                
                int pageNumber = int.Parse(pageTextUI.text);
                Button pageNumberButton = pageItemTransform.GetComponent<Button>();
                pageNumberButton.onClick.AddListener(() => LoadSpecificPage(pageNumber));
            }
        }
   }

   public async void LoadSpecificPage(int pageNumber)
   {
     await LoadCharacters(pageNumber);
   }


    public async void NextPage()
    {
        currentPage++;
        await LoadCharacters(currentPage);
    }

    public async void PreviousPage()
    {
        currentPage = Mathf.Max(1, currentPage - 1);
        await LoadCharacters(currentPage);
    }

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
  
}



