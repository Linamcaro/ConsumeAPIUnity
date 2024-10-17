using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Prueba.ApiService
{
    public class ApiRequest
    {
        private readonly string apiUrl = "https://rickandmortyapi.com/api/character?page={0}";

        public async Task<CharacterResponse> GetCharacter(int page)
        {
            //create the apiUrl with the webpage
            string url = string.Format(apiUrl, page);
            
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                var operation = request.SendWebRequest();

                while(!operation.isDone)
                {
                    await Task.Yield();
                }
                
                //Check if there are any error trying to comunicate with the server or returned from the server
                if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                    return null;
                }

                return JsonUtility.FromJson<CharacterResponse>(request.downloadHandler.text);

            }
        }

    }
}
