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

   private CharacterResponse response;

   // Instance of ApiService
   private ApiRequest apiRequest;

   private int currentPage = 1;


   private void Awake() {

    characterItemTemplate.gameObject.SetActive(false);
    pageItemTemplate.gameObject.SetActive(false);

    apiRequest = new ApiRequest();
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

                //Duplicate the item template inside the container
                Transform characterItemTransform = Instantiate(characterItemTemplate, characterContainer);
                RectTransform characterItemRectTransform = characterItemTransform.GetComponent<RectTransform>(); 
                    
                //set the text
                characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = character.name;

                characterItemTransform.gameObject.SetActive(true);

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

  
}



