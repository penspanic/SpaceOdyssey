using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetFactory : MonoBehaviour
{

    public GameObject basicPlanet;
    public GameObject[] dynamicPlanets;

    DataManager dm;
    LanguageManager languageMgr;
    void Awake()
    {
        dm = GameObject.FindObjectOfType<DataManager>();
        languageMgr = GameObject.FindObjectOfType<LanguageManager>();
    }
    public PlanetBase GetNewPlanet(int currentSector)
    {
        PlanetBase newPlanet;
        if (Random.Range(0, 10) < 2)
        {
            newPlanet = Instantiate<GameObject>(dynamicPlanets[Random.Range(0, dynamicPlanets.Length)]).GetComponent<SpecialPlanet>();
        }
        else
        {
            newPlanet = Instantiate<GameObject>(basicPlanet).GetComponent<PlanetBase>();
        }

        int rand = Random.Range(0, 100);
        bool canSurvive = false;
        if (rand < dm.GetCurrentSectorData(currentSector).successRate)
            canSurvive = true;
        else
            canSurvive = false;

        newPlanet.SetData(GetScript(canSurvive), canSurvive);

        return newPlanet.GetComponent<PlanetBase>();
    }
    public string[] GetScript(bool canSurvive)
    {
        List<string> scriptList = new List<string>();
        if (canSurvive)
        {
            int survivableScriptNum = Random.Range(2, 4);
            for (int i = 0; i < survivableScriptNum; i++)
            {
                while (true)
                {
                    string newScript = null;
                    if (languageMgr.currLanguage == Language.Korean)
                        newScript = dm.survivablePlanetScript[Random.Range(0, dm.survivableScriptNum)];
                    else
                        newScript = dm.engSurvivablePlanetScript[Random.Range(0, dm.survivableScriptNum)];

                    if (scriptList.Contains(newScript))
                        continue;
                    else
                    {
                        scriptList.Add(newScript);
                        break;
                    }
                }
            }
            for (int i = 0; i < 3 - survivableScriptNum; i++)
            {
                while (true)
                {
                    string newScript = null;
                    if (languageMgr.currLanguage == Language.Korean)
                        newScript = dm.nonviablePlanetScript[Random.Range(0, dm.nonviableScriptNum)];
                    else
                        newScript = dm.engNonviablePlanetScript[Random.Range(0, dm.nonviableScriptNum)];

                    if (scriptList.Contains(newScript))
                        continue;
                    else
                    {
                        scriptList.Add(newScript);
                        break;
                    }
                }
            }
        }
        else
        {
            int nonvableScriptNum = Random.Range(2, 4);
            for (int i = 0; i < nonvableScriptNum; i++)
            {
                while (true)
                {
                    string newScript = null;
                    if (languageMgr.currLanguage == Language.Korean)
                        newScript = dm.nonviablePlanetScript[Random.Range(0, dm.nonviableScriptNum)];
                    else
                        newScript = dm.engNonviablePlanetScript[Random.Range(0, dm.nonviableScriptNum)];

                    if (scriptList.Contains(newScript))
                        continue;
                    else
                    {
                        scriptList.Add(newScript);
                        break;
                    }
                }
            }
            for (int i = 0; i < 3 - nonvableScriptNum; i++)
            {
                while (true)
                {
                    string newScript = null;
                    if (languageMgr.currLanguage == Language.Korean)
                        newScript = dm.survivablePlanetScript[Random.Range(0, dm.survivableScriptNum)];
                    else
                        newScript = dm.engSurvivablePlanetScript[Random.Range(0, dm.survivableScriptNum)];

                    if (scriptList.Contains(newScript))
                        continue;
                    else
                    {
                        scriptList.Add(newScript);
                        break;
                    }
                }
            }
        }
        return scriptList.ToArray();
    }
    public void ResetScriptLanguage(PlanetBase planet)
    {
        if (planet.name == "Earth")
        {
            string[] engEarthScript = new string[3] { "Home of Human race.", "Critical pollution detected.", "Serious resource exhaustion." };
            if (languageMgr.currLanguage == Language.English)
                planet.SetData(engEarthScript, false);
            return;
        }
        string[] newScripts = GetScript(planet.canSurvive);
        planet.SetData(newScripts, planet.canSurvive);
    }
}