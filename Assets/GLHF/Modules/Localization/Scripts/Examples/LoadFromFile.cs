using UnityEngine;
using GLHF.LanguageSystemV1;

public class LoadFromFile : MonoBehaviour
{
    [SerializeField] private TextAsset configEnglish;
    [SerializeField] private TextAsset configRussian;
    
    private void Start()
    {
        //Define default language in Start method after
        //OnLanguageSet event registration on Awake
        LoadLanguageFromFile(configEnglish);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            LoadLanguageFromFile(configEnglish);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadLanguageFromFile(configRussian);
        }
    }

    private void LoadLanguageFromFile(TextAsset configFile)
    {
        if(configFile == null)
        {
            Debug.Log("Language file is null!");
            return;
        }

        try
        {
            var defaultLanguage = configFile.ToString();
            var languageTokenList = JsonUtility.FromJson<LanguagePackage>(defaultLanguage);
            LanguageUtility.SetLanguage(languageTokenList);
        }
        catch
        {
            Debug.Log("Could not parse language file");
        }
    }
}
