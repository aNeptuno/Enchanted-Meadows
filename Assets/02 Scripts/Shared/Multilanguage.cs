using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

public class Multilanguage : MonoBehaviour
{
    #region "Singleton / Language init"
    public static Multilanguage Instance { get; private set; } // Singleton instance

    public LocalizeTextTMP cropNameLocalization;
    public TextMeshProUGUI translatedCrop;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }

        // -- Language
        LocalizationManager.Read();

        if (PlayerPrefs.GetString("Language") != null)
            ChangeLanguage(PlayerPrefs.GetString("Language"));
        else
            CurrentLang = Languages.EN;
    }
    #endregion
    public enum Languages
    {
        EN, //English
        SP //Spanish
    }

    Languages currentLang;
    public Languages CurrentLang {
        set {
            currentLang = value;

            switch(currentLang)
            {
                case Languages.EN:
                ChangeLanguage("English");
                PlayerPrefs.SetString("Language","English");
                break;
                case Languages.SP:
                ChangeLanguage("Spanish");
                PlayerPrefs.SetString("Language","Spanish");
                break;
            }
        }
        get => currentLang;
    }

    public void ChangeLanguage(string lan)
    {
        LocalizationManager.Language = lan;
    }

    public string GetCropName()
    {
        return translatedCrop.text;
    }

    public void UpdateCropName(string cropID) => cropNameLocalization.LocalizationKey = cropID;
}
