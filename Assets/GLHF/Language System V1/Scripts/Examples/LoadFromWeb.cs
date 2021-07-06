using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using GLHF.LanguageSystemV1;

public class LoadFromWeb : MonoBehaviour
{
    [SerializeField] 
    private string serverURL;

    public void GetLanguageFromWeb()
    {
        StartCoroutine(RequestLanguage());
    }

    private IEnumerator RequestLanguage()
    {
        var webRequest = UnityWebRequest.Get(serverURL);
        //webRequest.SetRequestHeader("headerName", "headerValue"); // If needed
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                var downloadText = webRequest.downloadHandler.text;
                var tokenList = JsonUtility.FromJson<LanguagePackage>(downloadText);
                LanguageUtility.SetLanguage(tokenList);
                break;

            default:
                Debug.Log("Language web request failed");
                break;
        }
    }
}
