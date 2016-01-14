using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Language
{
    Korean,
    English
}

public class LanguageManager : MonoBehaviour
{
    public Language currLanguage
    {
        get;
        private set;
    }

    public List<IUseLanguage> languageUsableObjectList = new List<IUseLanguage>();

    void Awake()
    {

    }

    public void ChangeLanguage(Language language)
    {
        switch (language)
        {
            case Language.Korean:
                currLanguage = Language.Korean;
                break;
            case Language.English:
                currLanguage = Language.English;
                break;
        }
        DataLoadSave.SetString("Language", language == Language.Korean ? "Korean" : "English");
        PlayerPrefs.Save();
    }
    public string Translate(string s)
    {
        if (currLanguage == Language.English)
        {
            if (s == "이주하기")
                return "Emigrate";
            else if (s == "탐사하기")
                return "Explore";
            else if (s == "광년")
                return "LY";
            else if (s == "명")
                return "Ppl";
            else
                return null;
        }
        else
            return s;

    }
}
