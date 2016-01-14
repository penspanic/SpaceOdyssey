using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

    GameManager gameMgr;
    SectorManager sectorMgr;
    BGMManager bgmMgr;

    public Image hpBar;

    public Sprite normalHpSprite;
    public Sprite dangerHpSprite;

    public Text hp;
    public float sectorTime;
    public float maxHp;
    public float currentHp;

    public PlanetBase mainPlanet;

    SectorData sectorData;

    UIManager um;

    void Awake()
    {
        gameMgr = GameObject.FindObjectOfType<GameManager>();
        sectorMgr = GameObject.FindObjectOfType<SectorManager>();
        bgmMgr = GameObject.FindObjectOfType<BGMManager>();

        maxHp = 100;
        currentHp = maxHp;
        um = GameObject.FindObjectOfType<UIManager>();
    }

    bool isEmergencyBgm = false;
    int prevHp;
    void Update()
    {

        if (gameMgr.isStart)
        {
            currentHp -= Time.deltaTime * 1f / sectorData.hpTime;
            hpBar.fillAmount = (float)currentHp / (float)maxHp;
            hp.text = ((int)currentHp).ToString() + "/" + ((int)maxHp).ToString();
            if(currentHp<=20 && !isEmergencyBgm)
            {
                isEmergencyBgm = true;
                bgmMgr.OnHpEnergency();
            }
            else if(currentHp>=20 && isEmergencyBgm)
            {
                isEmergencyBgm = false;
                bgmMgr.OnHpHealed();
            }
            if (currentHp / maxHp >= 0.5f)
            {
                hpBar.sprite = normalHpSprite;
            }
            else
            {
                hpBar.sprite = dangerHpSprite;
            }

            if (currentHp < 0)
                GameOver();
        }
    }

    void GameOver()
    {
        gameMgr.GameOver();
        gameObject.SetActive(false);
    }

    public void UpdateSectorData(SectorData data)
    {
        sectorData = data;
    }
    public void Emigration(PlanetBase target)
    {
        this.mainPlanet = target;
        for (int i = 0; i < mainPlanet.transform.parent.childCount;i++ )
        {
            if (mainPlanet.transform.parent.GetChild(i) != mainPlanet.transform)
            {
                mainPlanet.transform.parent.GetChild(i).SetParent(null);
                i--;
            }
        }
        sectorMgr.OnEmigration(target.transform.parent.GetComponent<Sector>().nowSectorTile.index - 1);

        if (target.canSurvive)
            currentHp = maxHp;
        else
            currentHp -= 10;
        um.EmigrationEffect(target.canSurvive);
    }
}
