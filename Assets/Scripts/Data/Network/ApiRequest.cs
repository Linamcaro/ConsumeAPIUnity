using UnityEngine.Networking;

namespace Test.ApiService
{
    public class ApiRequest
    {
        public UnityWebRequest GetCharacter(string url)
        {
            var request = new UnityWebRequest(url);

            request.downloadHandler = new DownloadHandlerBuffer();

            return request;
        }
    }
}

