using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public bool isStart = false;
    bool isEnd;

    public Camera mainCamera;
    public Transform cameraPosition;
    public GameObject title;
    public SpriteRenderer[] stars;
    bool[] isStarsVisible;
    float[] starsET;

    public AudioClip gameStartSound;

    public SpriteRenderer line;
    public Transform background;
    public GameObject creditText;

    public GameObject additionalObjects;
    public GameObject moon;
    public GameObject satellite;
    public GameObject satellite2;
    public GameObject tutorialRocket;

    AudioSource audioSource;
    SectorManager sectorMgr;
    UIManager uiMgr;
    BGMManager bgmMgr;
    Tutorial tutorial;

    public int score;

    void Awake()
    {
        
        Screen.SetResolution(1280, 720, true);
        audioSource = GetComponent<AudioSource>();
        sectorMgr = GameObject.FindObjectOfType<SectorManager>();
        uiMgr = GameObject.FindObjectOfType<UIManager>();
        bgmMgr = GameObject.FindObjectOfType<BGMManager>();
        tutorial = GameObject.FindObjectOfType<Tutorial>();
        Application.targetFrameRate = 60;
        line.color = Color.clear;

        isStarsVisible = new bool[stars.Length];
        starsET = new float[stars.Length];

        Ranking.instance.CheckInstance();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            EscapeProcess();
        if (isEnd)
        {
            if (Input.GetMouseButtonDown(0) && Time.timeScale == 1)
                SceneFader.GetInstance().ChangeScene(Application.loadedLevel);
        }

        for (int i = 0; i < stars.Length; i++)
        {
            starsET[i] += Time.deltaTime * (i + 1) / 10f;
            if (isStarsVisible[i])
            {
                stars[i].color = Color.Lerp(stars[i].color, Color.clear, starsET[i]);
                if (stars[i].color == Color.clear)
                {
                    isStarsVisible[i] = false;
                    starsET[i] = 0;
                }
            }
            else
            {
                stars[i].color = Color.Lerp(stars[i].color, Color.white, starsET[i]);
                if (stars[i].color == Color.white)
                {
                    isStarsVisible[i] = true;
                    starsET[i] = 0;
                }
            }
        }

        background.Rotate(Vector3.forward * Time.deltaTime * 5f);
    }

    void EscapeProcess()
    {
        Application.Quit();
    }

    public void OnGameStart()
    {
        isStart = true;
        StartCoroutine(GameStart());
        audioSource.clip = gameStartSound;
        audioSource.Play();
        Destroy(title);
        Destroy(moon);
        Destroy(satellite);
        Destroy(satellite2);
    }
    const float cameraOriginalSize = 1.4f;
    IEnumerator GameStart()
    {
        tutorial.ShowTutorial(true);
        while(true)
        {
            if (tutorial.isTutorialEnd)
                break;
            else
                yield return null;
        }
        Destroy(tutorialRocket);
        float elapsedTime = 0;
        float et = 0;

        PlanetBase[] planets = GameObject.FindObjectsOfType<PlanetBase>();

        PlanetFactory pf = GameObject.FindObjectOfType<PlanetFactory>();
        for (int i = 0; i < planets.Length; i++)
            pf.ResetScriptLanguage(planets[i]);

        Time.timeScale = 0;
        while (true)
        {
            yield return null;

            mainCamera.orthographicSize += 0.042f;
            if (mainCamera.orthographicSize > 3.6f)
            {
                mainCamera.orthographicSize = 3.6f;
                elapsedTime += 0.0016f;
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, elapsedTime);
            }
            if (mainCamera.transform.position == cameraPosition.position)
            {
                et += 0.0042f;
                line.color = Color.Lerp(line.color, Color.white, et);
            }
            if (line.color == Color.white)
                break;
        }
        Time.timeScale = 1;
        uiMgr.MoveBotomUI();
        bgmMgr.StopIntroBGM();

    }

    public void GameOver()
    {
        Ranking.instance.NewScore(score);
        uiMgr.OnGameEnd();
        bgmMgr.OnGameOver();
        isEnd = true;

    }
}