using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class SectorData
{
    public int firstSectorNum; // 이 섹터 데이터의 첫 섹터
    public int lastSectorNum;  // 이 섹터 데이터의 마지막 섹터
    public int successRate;
    public int minimumSuccessRate;
    public float hpTime;
    public int population;
    public int sectorMovePoint;
}

public class DataManager : MonoBehaviour
{

    public TextAsset dataFile;

    public string[] survivablePlanetScript;
    public string[] nonviablePlanetScript;

    public string[] engSurvivablePlanetScript;
    public string[] engNonviablePlanetScript;


    public int survivableScriptNum;
    public int nonviableScriptNum;

    JsonData dataObject;

    List<SectorData> sectorDataList = new List<SectorData>();

    static bool isInstanceExist;
    void Awake()
    {
        if(isInstanceExist)
        {
            Destroy(this.gameObject);
            return;
        }
        isInstanceExist = true;
        DontDestroyOnLoad(this.gameObject);
        LoadData();
        survivableScriptNum = survivablePlanetScript.Length;
        nonviableScriptNum = nonviablePlanetScript.Length;
    }

    void LoadData()
    {

        dataObject = JsonMapper.ToObject(dataFile.text);

        survivablePlanetScript = new string[dataObject["Survivable Planet Script"].Count];
        engSurvivablePlanetScript = new string[dataObject["English Survivable Planet Script"].Count];
        for (int i = 0; i < dataObject["Survivable Planet Script"].Count; i++)
        {
            survivablePlanetScript[i] = dataObject["Survivable Planet Script"][i].ToString();
            engSurvivablePlanetScript[i] = dataObject["English Survivable Planet Script"][i].ToString();
        }

        nonviablePlanetScript = new string[dataObject["Nonviable Planet Script"].Count];
        engNonviablePlanetScript = new string[dataObject["English Nonviable Planet Script"].Count];
        for (int i = 0; i < dataObject["Nonviable Planet Script"].Count; i++)
        {
            nonviablePlanetScript[i] = dataObject["Nonviable Planet Script"][i].ToString();
            engNonviablePlanetScript[i] = dataObject["English Nonviable Planet Script"][i].ToString();
        }

        SectorData newSectorData;
        JsonData rawSectorData;
        for (int i = 0; i < dataObject["Sector Data"].Count; i++)
        {
            newSectorData = new SectorData();
            rawSectorData = dataObject["Sector Data"][i];
            newSectorData.firstSectorNum = int.Parse(rawSectorData["First Sector Number"].ToString());
            newSectorData.lastSectorNum = int.Parse(rawSectorData["Last Sector Number"].ToString());
            newSectorData.successRate = int.Parse(rawSectorData["Success Rate"].ToString());
            newSectorData.minimumSuccessRate = int.Parse(rawSectorData["Minimum Success Rate"].ToString());
            newSectorData.hpTime = float.Parse(rawSectorData["Hp Time"].ToString());
            newSectorData.population = int.Parse(rawSectorData["Population"].ToString());
            newSectorData.sectorMovePoint = int.Parse(rawSectorData["Sector Move Point"].ToString());

            sectorDataList.Add(newSectorData);
        }
    }

    public SectorData GetCurrentSectorData(int sectorNum)
    {
        for (int i = 0; i < sectorDataList.Count; i++)
        {
            if (sectorNum >= sectorDataList[i].firstSectorNum && sectorNum <= sectorDataList[i].lastSectorNum)
            {
                return sectorDataList[i];
            }
        }
        return null;
    }

}