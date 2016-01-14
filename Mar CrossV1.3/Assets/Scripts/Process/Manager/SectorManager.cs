using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SectorTile
{
    public int index;
    public Vector2 firstSeat;
    public Vector2 secondSeat;
    public Vector2 thirdSeat;
    public Vector2 fourthSeat;
    public Vector2 fifthSeat;
    public float x;
    public Sector onSector;
}

public class SectorManager : MonoBehaviour
{
    public SectorData sectorData;

    public int currentSector;

    public Sector startSector;

    public Sector sector;
    Sector[] sectors = new Sector[6];
    int sectorIndex = 0;


    public PlanetBase planet;

    public GameObject currPlanetHighlight;

    public SectorTile[] sectorTiles = new SectorTile[6];

    public int moveSectorNum = 0;

    PlanetFactory planetFactory;

    DataManager dm;
    GameManager gm;

    Player player;

    void Start()
    {
        planetFactory = GameObject.FindObjectOfType<PlanetFactory>();
        gm = GameObject.FindObjectOfType<GameManager>();
        dm = GameObject.FindObjectOfType<DataManager>();
        player = GameObject.FindObjectOfType<Player>();
        
        for (int i = 0; i < sectorTiles.Length; i++)
        {
            sectorTiles[i] = new SectorTile();
            sectorTiles[i].index = i;
            //sectorTiles[i].x = -8f + 3.2f * i;
            sectorTiles[i].onSector = null;
        }
        sectorTiles[0].firstSeat = new Vector2(-1000, -800)/100f;
        sectorTiles[0].secondSeat = new Vector2(-1000, -800) / 100f;
        sectorTiles[0].thirdSeat = new Vector2(-1000, -800) / 100f;
        sectorTiles[0].fourthSeat = new Vector2(-1000, -800) / 100f;
        sectorTiles[0].fifthSeat = new Vector2(-1000, -800) / 100f;

        sectorTiles[1].firstSeat = new Vector2(-800, -500) / 100f;
        sectorTiles[1].secondSeat = new Vector2(-800, -500) / 100f;
        sectorTiles[1].thirdSeat = new Vector2(-512, -198) / 100f;
        sectorTiles[1].fourthSeat = new Vector2(-800, -500) / 100f;
        sectorTiles[1].fifthSeat = new Vector2(-800, -500) / 100f;

        sectorTiles[2].firstSeat = new Vector2(-512, 114) / 100f;
        sectorTiles[2].secondSeat = new Vector2(-386, 96) / 100f;
        sectorTiles[2].thirdSeat = new Vector2(-255, 10) / 100f;
        sectorTiles[2].fourthSeat = new Vector2(-188, -117) / 100f;
        sectorTiles[2].fifthSeat = new Vector2(-175, -207) / 100f;

        sectorTiles[3].firstSeat = new Vector2(-23, 255) / 100f;
        sectorTiles[3].secondSeat = new Vector2(47, 139) / 100f;
        sectorTiles[3].thirdSeat = new Vector2(90, 64) / 100f;
        sectorTiles[3].fourthSeat = new Vector2(119, -30) / 100f;
        sectorTiles[3].fifthSeat = new Vector2(134, -117) / 100f;

        sectorTiles[4].firstSeat = new Vector2(368, 258) / 100f;
        sectorTiles[4].secondSeat = new Vector2(430, 150) / 100f;
        sectorTiles[4].thirdSeat = new Vector2(452, 80) / 100f;
        sectorTiles[4].fourthSeat = new Vector2(470, -10) / 100f;
        sectorTiles[4].fifthSeat = new Vector2(486, -95) / 100f;

        sectorTiles[5].firstSeat = new Vector2(800, 500) / 100f;
        sectorTiles[5].secondSeat = new Vector2(800, 500) / 100f;
        sectorTiles[5].thirdSeat = new Vector2(800, 500) / 100f;
        sectorTiles[5].fourthSeat = new Vector2(800, 500) / 100f;
        sectorTiles[5].fifthSeat = new Vector2(800, 500) / 100f;

        for (int i = 0; i < sectors.Length; i++)
        {
            sectors[i] = Instantiate(sector);
            sectors[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < 6; i++)
        {
            if (i == 1)
                continue;
            Sector s = CreateSector();
            s.SetSectorTile(sectorTiles[i]);
            for (int j = 0; j < s.transform.childCount; j++)
                s.transform.GetChild(j).position = s.transform.GetChild(j).GetComponent<PlanetBase>().targetPos;
        }
        startSector.SetSectorTile(sectorTiles[1]);
        for (int i = 0; i < startSector.transform.childCount; i++)
            startSector.transform.GetChild(i).position = startSector.transform.GetChild(i).GetComponent<PlanetBase>().targetPos;

        ProbabilityRevision();

        sectorData = dm.GetCurrentSectorData(currentSector);
    }

    void Update()
    {
        if (player.mainPlanet != null&&gm.isStart)
        {
            currPlanetHighlight.SetActive(true);
            currPlanetHighlight.transform.localScale = player.mainPlanet.transform.localScale * 1.5f;
            currPlanetHighlight.transform.position = player.mainPlanet.transform.position;
        }
        else
            currPlanetHighlight.SetActive(false);
        if (sectorTiles[5].onSector == null)
        {
            CreateSector();
            if (moveSectorNum == 0)
                ProbabilityRevision();
        }
        for (int i = 0; i < sectorTiles.Length - 1; i++)
        {
            if (sectorTiles[i].onSector == null && sectorTiles[i + 1].onSector != null)
            {
                sectorTiles[i + 1].onSector.SetSectorTile(sectorTiles[i]);
                sectorTiles[i + 1].onSector = null;
            }
        }

        if (moveSectorNum > 0 &&
            sectorTiles[0].onSector != null &&
            ((Vector2)(sectorTiles[0].onSector.transform.GetChild(0).position)) == sectorTiles[0].onSector.transform.GetChild(0).GetComponent<PlanetBase>().targetPos)
        {
            sectorTiles[0].onSector = null;
            moveSectorNum--;
            gm.score += dm.GetCurrentSectorData(currentSector).sectorMovePoint;
        }

        if (sectorTiles[0].onSector != null && ((Vector2)(sectorTiles[0].onSector.transform.GetChild(0).position)) == sectorTiles[0].onSector.transform.GetChild(0).GetComponent<PlanetBase>().targetPos)
            sectorTiles[0].onSector.gameObject.SetActive(false);

        // if(sectorTiles[0].onSector!=null&&sectorTiles[0].onSector.transform.position.x == sectorTiles[0].x)
        //     sectorTiles[0].onSector.gameObject.SetActive(false);
    }

    public void OnplanetTouch(int line)
    {

    }

    void ProbabilityRevision()
    {
        int survivablePlanetCount = 0;
        int planetCount = 0;
        for(int i=2;i<=4;i++)
        {
            for (int j = 0; j < sectorTiles[i].onSector.transform.childCount; j++)
            {
                planetCount++;
                if (sectorTiles[i].onSector.transform.GetChild(j).GetComponent<PlanetBase>().canSurvive)
                    survivablePlanetCount++;
            }
        }

        while ((float)survivablePlanetCount / (float)planetCount < dm.GetCurrentSectorData(currentSector).minimumSuccessRate / 9.0f)
        {
            Sector s = sectorTiles[Random.Range(2, 5)].onSector;
            PlanetBase p = s.transform.GetChild(Random.Range(0, s.transform.childCount)).GetComponent<PlanetBase>();
            if (p.canSurvive == false&&p.isExplored == false)
            {
                p.SetData(planetFactory.GetScript(true), true);
                survivablePlanetCount++;
            }
        }
    }

    public void OnEmigration(int moveNum)
    {
        moveSectorNum = moveNum;
    }
    Sector CreateSector()
    {
        //StartCoroutine(HighlightCurrentPlanet());
        
        sectorData = dm.GetCurrentSectorData(currentSector);
        player.UpdateSectorData(sectorData);
        currentSector++;
        Sector nowSector = sectors[sectorIndex];
        while (nowSector.transform.childCount > 0)
        {
            Destroy(nowSector.transform.GetChild(0).gameObject);
            nowSector.transform.GetChild(0).SetParent(null);
        }

        int rand = Random.Range(1, 4);
        for (int i = 0; i < rand; i++)
        {
            PlanetBase p = planetFactory.GetNewPlanet(currentSector);
            p.gameObject.SetActive(true);
            p.transform.SetParent(nowSector.transform);
        }
        nowSector.gameObject.SetActive(true);
        nowSector.SetSectorTile(sectorTiles[5]);
        for (int i = 0; i < nowSector.transform.childCount;i++ )
            nowSector.transform.GetChild(i).position = nowSector.transform.GetChild(i).GetComponent<PlanetBase>().targetPos;

        sectorIndex++;
        if (sectorIndex == sectors.Length)
            sectorIndex = 0;

        return nowSector;
    }

    IEnumerator HighlightCurrentPlanet()
    {
        currPlanetHighlight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        currPlanetHighlight.SetActive(true);

        currPlanetHighlight.transform.position = sectorTiles[1].thirdSeat;

        StartCoroutine(CoroutineUtil.LerpColor(currPlanetHighlight.GetComponent<SpriteRenderer>(), new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 1f));
    }
}
