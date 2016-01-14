using UnityEngine;
using System.Collections;

public class PlanetBase : MonoBehaviour, ITouchable
{

    public bool isStartPlanet;
    protected SpriteRenderer spRenderer;
    protected SectorManager sectorMgr;
    protected UIManager uiMgr;

    public bool canSurvive;
    public bool isExplored;

    public static Sprite q_planet;
    public static Sprite[] planetSprites;
    public AudioClip effectSound;
    AudioSource adSource;
    protected Sprite planetSprite;

    public Vector2 targetPos;

    public string[] scripts;

    public bool isMove;

    protected float targetScale;
    protected bool isSpecial;

    protected virtual void Awake()
    {
        if(GetComponent<AudioSource>()!= null)
            adSource = GetComponent<AudioSource>();
        spRenderer = GetComponent<SpriteRenderer>();
        sectorMgr = GameObject.FindObjectOfType<SectorManager>();
        uiMgr = GameObject.FindObjectOfType<UIManager>();

        if (q_planet == null)
            q_planet = Resources.Load<Sprite>("s_p");
        if(planetSprites == null)
            planetSprites = Resources.LoadAll<Sprite>("Planets");
        planetSprite = planetSprites[Random.Range(0, planetSprites.Length)];

        if (isStartPlanet)
            targetScale = 0.45f;
        //spRenderer.enabled = false;
    }

    protected virtual void Update()
    {
        if (transform.parent == null)
            Destroy(gameObject);
        else if(isStartPlanet == false)
        {
            if (transform.parent.GetComponent<Sector>().nowSectorTile.index == 4)
                targetScale = 0.75f;
            else if (transform.parent.GetComponent<Sector>().nowSectorTile.index == 3)
                targetScale = 0.9f;
            else if (transform.parent.GetComponent<Sector>().nowSectorTile.index == 2)
                targetScale = 1.1f;
            else if (transform.parent.GetComponent<Sector>().nowSectorTile.index == 1)
                targetScale = 1.5f;
        }

        if(isMove == false)
        {
            isMove = true;
            StartCoroutine(CoroutineUtil.LerpMove(this.gameObject, this.transform.position, targetPos, 2));
            StartCoroutine(CoroutineUtil.LerpScale(this.gameObject, this.transform.localScale, new Vector3(1f, 1f, 1f)*targetScale, 2));
        }
    }

    public virtual void OnTouch()
    {
        //if (transform.parent.GetComponent<Sector>().nowSectorTile == sectorMgr.sectorTiles[1])
        //    return;
        if(adSource!= null)
            adSource.Play();
        uiMgr.OnPlanetTouch(this);
    }

    public virtual void Explore()
    {
        Debug.Log("PlanetBase : Explore");
        spRenderer.sprite = planetSprite;
        //spRenderer.enabled = true;
        isExplored = true;
    }

    public void SetData(string[] scripts, bool canSurvive)
    {
        this.canSurvive = canSurvive;
        this.scripts = scripts;
    }
}