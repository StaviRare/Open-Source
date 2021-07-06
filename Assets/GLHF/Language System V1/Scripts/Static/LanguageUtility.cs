using System;
using UnityEngine;
using TMPro;

namespace GLHF.LanguageSystemV1
{
    public static class LanguageUtility
    {
        public static event Action OnLanguageSet;
        private static LanguagePackage _languageContent;

        public static void SetText(TextMeshProUGUI textObject, string key)
        {
            textObject.text = GetLanguageValueFromKey(key);
            textObject.isRightToLeftText = _languageContent.IsRTL;
        }

        public static void SetLanguage(LanguagePackage languageList)
        {
            _languageContent = languageList;
            OnLanguageSet?.Invoke();
        }

        private static string GetLanguageValueFromKey(string key)
        {
            try
            {
                var foundValue = _languageContent.TokenList.Find((x) => x.Key == key).Value;
                return foundValue;
            }
            catch
            {
                Debug.Log("Could not find value (" + key + ")");
                return "Missing!";
            }
        }
    }
}