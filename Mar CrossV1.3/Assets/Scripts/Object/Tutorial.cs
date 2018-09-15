using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour, ITouchable
{
    public Image tutorialImage;

    public Sprite[] korSprites;
    public Sprite[] engSprites;

    LanguageManager languageMgr;
    GameManager gameMgr;

    bool tutorialShowed;
    public bool isTutorialEnd = true;
    public bool touchIntervalEnd = true;
    public int spriteIndex = 0;
    void Awake()
    {
        if(languageMgr == null)
            languageMgr = GameObject.FindObjectOfType<LanguageManager>();
        if(gameMgr == null)
            gameMgr = GameObject.FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && (!isTutorialEnd) && touchIntervalEnd)
        {
            TutorialProcess();
        }
    }

    public void OnTouch()
    {
        if (touchIntervalEnd)
            ShowTutorial(false);
    }

    void TutorialProcess()
    {
        if(spriteIndex == 0)
        {
            if (languageMgr.currLanguage == Language.Korean)
                tutorialImage.sprite = korSprites[1];
            else
                tutorialImage.sprite = engSprites[1];
            touchIntervalEnd = false;
            StartCoroutine(TouchInterval());
            spriteIndex++;
            return;
        }
        else if(spriteIndex == 1)
        {
            StartCoroutine(TutorialEnd());
            spriteIndex++;
        }
    }

    public void ShowTutorial(bool isCheck)
    {
        if (gameMgr == null)
            gameMgr = GameObject.FindObjectOfType<GameManager>();
        if (!isTutorialEnd)
            return;
        if(isCheck)
        {
            bool isPlayed = false;
            if (DataLoadSave.HasKey("Tutorial"))
                isPlayed = true;
            if (isPlayed)
                return;
        }
        DataLoadSave.SetInt("Tutorial", 0);
        StartCoroutine(TutorialStart());

        
    }
    IEnumerator TutorialStart()
    {
        isTutorialEnd = false;
        touchIntervalEnd = false;
        spriteIndex = 0;
        SceneFader.GetInstance().FadeOut();
        yield return new WaitForSeconds(1);
        SceneFader.GetInstance().FadeIn();
        tutorialImage.sprite = null;
        tutorialImage.gameObject.SetActive(true);
        if (languageMgr == null)
            languageMgr = GameObject.FindObjectOfType<LanguageManager>();
        if (languageMgr.currLanguage == Language.Korean)
        {
            tutorialImage.sprite = korSprites[0];
        }
        else
        {
            tutorialImage.sprite = engSprites[0];
        }
        StartCoroutine(TouchInterval());
    }
    IEnumerator TutorialEnd()
    {
        SceneFader.GetInstance().FadeOut();
        yield return new WaitForSeconds(1f);
        SceneFader.GetInstance().FadeIn();
        tutorialImage.gameObject.SetActive(false);
        if (gameMgr.isStart)
        {
            this.GetComponent<Image>().enabled = false;
            this.transform.Find("tutorial_t").GetComponent<Image>().enabled = false;
        }

        yield return new WaitForSeconds(0.25f);
        isTutorialEnd = true;
    }
    IEnumerator TouchInterval()
    {
        yield return new WaitForSeconds(1f);
        touchIntervalEnd = true;
    }
}