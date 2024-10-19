using System;
using System.Collections;
using System.Collections.Generic;
using Test.ApiService;
using UnityEngine;
using UnityEngine.Networking;

namespace Test.CharacterRepository
{
    public class CharacterRepository
    {
        #region ======== Event variables ========
        //Event that triggers when the data is retrieved
        public event Action<CharacterResponse> OnCharacterRequest;

        //Event that triggers when there is an connection error
        public event Action<string> OnWebRequestError;
        #endregion

        private ApiRequest apiRequest;
        private Dictionary<int, CharacterResponse> characterResponseCache;

        #region ======== Set Instance ========
        private static CharacterRepository instance;
        public static CharacterRepository Instance
        {
            get
            {
                if(instance == null) 
                {
                    instance = new CharacterRepository();
                }

                return instance;
            }
        }
        #endregion

        private CharacterRepository()
        {
            apiRequest = new ApiRequest();
            characterResponseCache = new Dictionary<int,CharacterResponse>();
        }

        
        /// <summary>
        /// Spawn a character item template
        /// </summary>
        /// <param name="page"></param>
        public IEnumerator LoadCharacters(int page)
        {    
            //Check if the page requested is cached
            if(characterResponseCache.ContainsKey(page))
            {
                OnCharacterRequest?.Invoke(characterResponseCache[page]);
                Debug.Log("page called from the chached");
                yield break;
            }
            else
            {
                //Format the URL to request with the page number requested
                string formatedUrl = string.Format(Constants.apiUrl, page);
                var requestData = apiRequest.GetCharacter(formatedUrl);

                yield return requestData.SendWebRequest();

                    //Check for connection errors
                if(requestData.result == UnityWebRequest.Result.ConnectionError || requestData.result == UnityWebRequest.Result.ProtocolError)
                {

                    Debug.LogError($"Connection failed: {requestData.error}");
                    OnWebRequestError?.Invoke(requestData.error); 
                  
                }
                else
                {
                    CharacterResponse response = JsonUtility.FromJson<CharacterResponse>(requestData.downloadHandler.text);
                    Debug.Log("API called");
                        
                    //if response is not null cache the response
                    if(response != null)
                    {
                        characterResponseCache[page] = response;
                        OnCharacterRequest?.Invoke(response);
                            
                    } 
                }  
               
            }
                
        }
    }

}