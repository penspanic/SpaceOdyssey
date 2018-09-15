using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour, IUseLanguage
{
    public Text[] descriptionTexts;
    public GameObject bottomUI;
    public Button selectButton;
    Text selectButtonText;
    public Image exploringRing;

    public Sprite emigrationSpriteOn;
    public Sprite emigrationSpriteOff;
    public Sprite exploreSpriteOn;
    public Sprite exploreSpriteOff;

    public Transform ringHome;

    public GameObject topUI;
    public GameObject gameEndUI;
    public GameObject energyImage;

    public GameObject selectedHighlight;

    public Text gameEndScoreText;

    public Sprite missEffectSprite;
    public Sprite rightEffectSprite;

    public Image effect;

    PlanetBase selectedPlanet;

    Player player;
    public Rocket rocket;

    DataManager dm;
    GameManager gm;

    SectorManager sectorMgr;
    LanguageManager languageMgr;

    public AudioClip exploringSound;
    public AudioClip exploreSound;
    public AudioClip exploreSuccessSound;
    public AudioClip emigrationSound;
    public AudioClip emigrationSucessSound;
    public AudioClip emigrationFailSound;

    AudioSource audioSource;
    public Text population;
    public Text score;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<Player>();
        dm = GameObject.FindObjectOfType<DataManager>();
        gm = GameObject.FindObjectOfType<GameManager>();
        sectorMgr = GameObject.FindObjectOfType<SectorManager>();
        languageMgr = GameObject.FindObjectOfType<LanguageManager>();
        selectButtonText = selectButton.GetComponentInChildren<Text>();

        NotifyLanguageUsable();
    }

    bool selectedPlanetIsCurrentPlanet = false;

    void Update()
    {
        if (selectedPlanet != null)
        {
            selectedHighlight.SetActive(true);
            selectedHighlight.transform.position = selectedPlanet.transform.position;
            selectedHighlight.transform.localScale = selectedPlanet.transform.localScale * 1.3f;

            if (selectedPlanetIsCurrentPlanet)
            {
                if (selectedPlanet.name == "Earth")
                    selectedHighlight.transform.localScale = Vector3.one * 1.7f;
                SetDescription(selectedPlanet.scripts, TextAnchor.MiddleLeft);
                selectButtonText.text = "";
                energyImage.SetActive(false);
                return;
            }
            if (selectedPlanet.isExplored)
            {
                energyImage.SetActive(false);
                selectButton.image.sprite = emigrationSpriteOn;
                SpriteState s = new SpriteState();
                s.pressedSprite = emigrationSpriteOff;
                selectButton.spriteState = s;
                selectButtonText.text = languageMgr.Translate("이주하기");
                selectButtonText.color = Color.black;
                SetDescription(selectedPlanet.scripts, TextAnchor.MiddleLeft);
            }
            else
            {
                energyImage.SetActive(true);
                selectButton.image.sprite = exploreSpriteOn;
                SpriteState s = new SpriteState();
                s.pressedSprite = exploreSpriteOff;
                selectButton.spriteState = s;
                int distance = selectedPlanet.transform.parent.GetComponent<Sector>().nowSectorTile.index - 1;
                selectButtonText.color = Color.white;
                selectButtonText.text = languageMgr.Translate("탐사하기") + "\n-" + (5f * distance).ToString();
                SetDescription(new string[] { "? ? ?", "? ? ?", "? ? ?" }, TextAnchor.MiddleCenter);
            }
        }
        else
        {
            energyImage.SetActive(false);
            selectedHighlight.SetActive(false);
        }
        population.text = dm.GetCurrentSectorData(sectorMgr.currentSector).population.ToString() + languageMgr.Translate("명");
        score.text = gm.score.ToString() + languageMgr.Translate("광년");
    }

    void SetDescription(string[] descriptions, TextAnchor anchor)
    {
        for (int i = 0; i < 3; i++)
        {
            descriptionTexts[i].text = descriptions[i];
            descriptionTexts[i].alignment = anchor;
        }
    }
    public void NotifyLanguageUsable()
    {
        GameObject.FindObjectOfType<LanguageManager>().languageUsableObjectList.Add(this);
    }

    public void LanguageChanged()
    {

    }

    public void MoveBotomUI()
    {
//        StartCoroutine(CoroutineUtil.LerpMove(
//            bottomUI, bottomUI.transform.position, new Vector2(bottomUI.transform.position.x, bottomUI.transform.position.y + 1.91f),
//            3, false, false, this.gameObject, "OnMoveEnd"));
//        StartCoroutine(CoroutineUtil.LerpMove(
//            topUI, topUI.transform.position, new Vector2(topUI.transform.position.x, topUI.transform.position.y - 1.2f),
//            3, false, false, this.gameObject, "OnMoveEnd"));
        StartCoroutine(AnchorMove(bottomUI.GetComponent<RectTransform>(), new Vector2(-549f / 2f, 191f / 2f), 0.33f));
        StartCoroutine(AnchorMove(topUI.GetComponent<RectTransform>(), new Vector2(0f, -60f), 0.33f));
    }

    private IEnumerator AnchorMove(RectTransform target, Vector2 endPosition, float time)
    {
        Debug.Log("Start anchored position : " + target.anchoredPosition);
        float elapsedTime = 0f;
        Vector2 startPosition = target.anchoredPosition;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            target.anchoredPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / time);
            yield return null;
        }

        target.anchoredPosition = endPosition;
        OnMoveEnd();
    }

    bool bottomUIMoveEnd = false;
    bool hpBarMoveEnd = false;
    public void OnMoveEnd()
    {
        bottomUIMoveEnd = true;
        hpBarMoveEnd = true;
    }
    Queue<PlanetBase> exploringPlanets = new Queue<PlanetBase>();
    public bool isEmigrationEnd = true;

    public void OnSelectButtonDown()
    {
        if (selectedPlanet == null)
            return;
        if (!isEmigrationEnd)
            return;
        if (player.mainPlanet == selectedPlanet)
            return;
        audioSource.clip = exploreSound;
        audioSource.Play();
        if (selectedPlanet.isExplored)
        {
            rocket.Emigration(selectedPlanet);
            audioSource.clip = emigrationSound;
            audioSource.Play();
            selectedPlanet = null;
            selectButtonText.text = "";
            for (int i = 0; i < 3; i++)
                descriptionTexts[i].text = "";
        }
        else
        {
            if (exploringPlanets.Contains(selectedPlanet))
                return;
            StartCoroutine(ExploringProcess(selectedPlanet));
            exploringPlanets.Enqueue(selectedPlanet);
            OnPlanetTouch(selectedPlanet);
        }
    }

    public void OnExitButtonDown()
    {
        SceneFader.GetInstance().ChangeScene(Application.loadedLevel);
    }

    IEnumerator ExploringProcess(PlanetBase target)
    {
        int distance = selectedPlanet.transform.parent.GetComponent<Sector>().nowSectorTile.index - 1;
        player.currentHp -= 5f * distance;

        
        Image ring = Instantiate(exploringRing); // Create red ring that shows exploring progress.
        ring.transform.SetParent(ringHome, false);
        ring.transform.position = selectedPlanet.transform.position;
        ring.transform.localScale = target.transform.localScale;
        ring.GetComponent<Ring>().targetPlanet = selectedPlanet;
        ring.GetComponent<Ring>().isChasing = true;
        ring.fillAmount = 0;


        Text ringText = ring.GetComponentInChildren<Text>(); 
        float elapsedTime = 0;
        bool isSoundEffected = false;
        while (true)
        {
            if (player.gameObject.activeSelf == false)
                break;
            if (elapsedTime >= 2f)
                break;
            if (elapsedTime > 0.3f && !isSoundEffected)
            {
                isSoundEffected = true;
                audioSource.clip = exploringSound;
                audioSource.Play();
            }
            if (ring.transform.position.x <= -6.4f) // If ring become out of screen,
            {
                Destroy(ring);
                break;
            }
            ring.fillAmount += Time.deltaTime / 2;
            ringText.text = ((int)(ring.fillAmount * 100)).ToString() + "%"; // Show percent of exploring progress.
            elapsedTime += Time.deltaTime;
            exploringRing.transform.localScale = target.transform.localScale;
            yield return null;
        }
        Destroy(ring.gameObject);
        audioSource.Stop();
        audioSource.clip = exploreSuccessSound;
        audioSource.Play();
        PlanetBase destroyingObj = exploringPlanets.Dequeue(); // Destroy first added planet among exploring planets.
        destroyingObj.Explore();
    }

    public void OnPlanetTouch(PlanetBase planet)
    {

        if (!bottomUIMoveEnd || !hpBarMoveEnd)
            return;

        selectedPlanet = planet;

        if (planet.transform.parent.GetComponent<Sector>().nowSectorTile == sectorMgr.sectorTiles[1]
            && selectedPlanet)
        {
            selectedPlanetIsCurrentPlanet = true;
            return;
        }
        else
            selectedPlanetIsCurrentPlanet = false;


    }

    public void OnGameEnd()
    {
        SceneFader.GetInstance().FadeOut();
        StartCoroutine(EndGame());
    }
    IEnumerator EndGame()
    {
        while (true)
        {
            yield return null;
            if (SceneFader.GetInstance().fadeObjectImage.color == Color.black)
            {
                SceneFader.GetInstance().FadeIn();
                gameEndUI.SetActive(true);
                gameEndScoreText.text = "score\n" + (GameObject.FindObjectOfType<GameManager>().score).ToString();
                break;
            }
        }
    }

    public void EmigrationEffect(bool isRight)
    {
        if (isRight)
        {
            effect.sprite = rightEffectSprite;
            audioSource.clip = emigrationSucessSound;
            audioSource.Play();
        }
        else
        {
            effect.sprite = missEffectSprite;
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.23f, 0.4f);
            audioSource.clip = emigrationFailSound;
            audioSource.Play();
        }
        StartCoroutine(EffectProcessing());

    }

    IEnumerator EffectProcessing()
    {
        bool change = false;
        effect.gameObject.SetActive(true);
        float et = 0;
        Color target = new Color(1, 1, 1, 0);
        effect.color = target;
        while (true)
        {
            yield return null;
            et += Time.deltaTime * 3f;
            if (change == false)
            {
                effect.color = Color.Lerp(effect.color, Color.white, et);
                if (effect.color == Color.white)
                {
                    change = true;
                    et = 0;
                }
            }
            else
            {
                effect.color = Color.Lerp(effect.color, target, et);
                if (effect.color == target)
                    break;
            }
        }
        effect.gameObject.SetActive(false);
    }
}

