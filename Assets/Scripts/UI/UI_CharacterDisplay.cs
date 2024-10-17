using TMPro;
using UnityEngine;

public class UI_CharacterDisplay : MonoBehaviour
{
    //Character list
   [SerializeField] private Transform characterContainer;
   [SerializeField] private Transform characterItemTemplate;

    //Pagination
   [SerializeField] private Transform pageContainer;
   [SerializeField] private Transform pageItemTemplate;


   private void Awake() {

    characterItemTemplate.gameObject.SetActive(false);
    pageItemTemplate.gameObject.SetActive(false);
   }

   private void Start() {

    CreateITem("Lina");
    CreateITem("Pablo");
    CreateITem("Camila");
    CreateITem("Carlos");
    CreateITem("Juliana");
    CreateITem("Andres");
    CreateITem("Esteban");
    CreateITem("Andrea");
    CreateITem("Kevyn");
    CreateITem("Natalia");
    CreateITem("Esteban");
    CreateITem("Andrea");
    CreateITem("Kevyn");
    CreateITem("Natalia");
     CreateITem("Natalia");
    CreateITem("Esteban");
    CreateITem("Andrea");
    CreateITem("Kevyn");
    CreateITem("Natalia");



    CreatePageButtonITem(1);
    CreatePageButtonITem(2);
    CreatePageButtonITem(3);
    CreatePageButtonITem(4);
    CreatePageButtonITem(5);
    CreatePageButtonITem(6);
    CreatePageButtonITem(7);
    CreatePageButtonITem(8);
    CreatePageButtonITem(9);
    CreatePageButtonITem(10);
    CreatePageButtonITem(11);
    CreatePageButtonITem(12);
    CreatePageButtonITem(13);
    CreatePageButtonITem(14);

   }


   /// <summary>
   /// Spawn a character item template
   /// </summary>
   /// <param name="intemName"></param>
   private void CreateITem(string intemName)
   {

     //Duplicate the item template inside the container
     Transform characterItemTransform = Instantiate(characterItemTemplate, characterContainer);
     RectTransform characterItemRectTransform = characterItemTransform.GetComponent<RectTransform>(); 
     //set the text
     characterItemTransform.Find("name").GetComponent<TextMeshProUGUI>().text = intemName;

     characterItemTransform.gameObject.SetActive(true);

     //Button button = characterItemTransform.GetComponent<Button>();

   }

      /// <summary>
   /// Spawn a character item template
   /// </summary>
   /// <param name="intemName"></param>
   private void CreatePageButtonITem(int pageNumber)
   {

     //Duplicate the item template inside the container
     Transform pageItemTransform = Instantiate(pageItemTemplate, pageContainer);
     RectTransform pageItemRectTransform = pageItemTransform.GetComponent<RectTransform>(); 
    
     //set the text
     pageItemTransform.Find("page").GetComponent<TextMeshProUGUI>().text = pageNumber.ToString();

     pageItemTransform.gameObject.SetActive(true);

     //Button button = characterItemTransform.GetComponent<Button>();

   }
}
