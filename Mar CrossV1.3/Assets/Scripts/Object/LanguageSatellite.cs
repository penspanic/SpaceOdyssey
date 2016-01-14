using UnityEngine;
using System.Collections;

public class LanguageSatellite : MonoBehaviour, ITouchable
{
    public Sprite korSprite;
    public Sprite engSprite;

    public SpriteRenderer languageShow;

    LanguageManager languageMgr;
    Tutorial tutorial;

    public float moveSpeed;
    public float moveDistance;

    float riseEndY;
    float fallEndY;

    void Awake()
    {
        languageMgr = GameObject.FindObjectOfType<LanguageManager>();
        tutorial = GameObject.FindObjectOfType<Tutorial>();

        riseEndY = transform.position.y + moveDistance / 2;
        fallEndY = transform.position.y - moveDistance / 2;


    }

    void Start()
    {
        if(DataLoadSave.HasKey("Language"))
        {
            Language lang = DataLoadSave.GetString("Language") == "Korean" ? Language.Korean : Language.English;
            languageMgr.ChangeLanguage(lang);
            ChangeLanguageSprite();
        }
    }

    bool isRising = true;
    void Update()
    {
        if (isRising)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y >= riseEndY)
                isRising = false;
        }
        else
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= fallEndY)
                isRising = true;
        }
    }

    public void OnTouch()
    {
        if (languageMgr.currLanguage == Language.English)
        {
            languageShow.sprite = korSprite;
            languageMgr.ChangeLanguage(Language.Korean);
        }
        else
        {
            languageShow.sprite = engSprite;
            languageMgr.ChangeLanguage(Language.English);
        }
    }

    void ChangeLanguageSprite()
    {
        if(languageMgr.currLanguage == Language.English)
        {
            languageShow.sprite = engSprite;
        }
        else
        {
            languageShow.sprite = korSprite;
        }
    }
}
