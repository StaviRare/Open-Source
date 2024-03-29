using UnityEngine;
using UnityEngine.UI;
using GLHF.LanguageSystemV1;

public class SetLanguageValue : MonoBehaviour
{
    [SerializeField] private Text menuItem01;
    [SerializeField] private Text menuItem02;
    [SerializeField] private Text menuItem03;

    private void Awake()
    {
        LanguageUtility.OnLanguageSet += OnLanguageSetEvent;
    }

    private void OnDestroy()
    {
        LanguageUtility.OnLanguageSet -= OnLanguageSetEvent;
    }

    private void OnLanguageSetEvent()
    {
        var item01Key = LanguageKeys.test_01;
        LanguageUtility.SetText(menuItem01, item01Key);

        var item02Key = LanguageKeys.test_02;
        LanguageUtility.SetText(menuItem02, item02Key);

        var item03Key = LanguageKeys.test_03;
        LanguageUtility.SetText(menuItem03, item03Key);
    }
}