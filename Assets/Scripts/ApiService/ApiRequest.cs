using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Test.ApiService
{
    public class ApiRequest
    {
        //Event that triggers when there is an connection error
        public event Action<string> OnWebRequestError;

        public UnityWebRequest GetCharacter(string url)
        {
            var request = new UnityWebRequest(url);

            //Check for connection errors
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
               Debug.LogError(request.error);

               OnWebRequestError?.Invoke(request.error);   
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            return request;
        }
    }
}

