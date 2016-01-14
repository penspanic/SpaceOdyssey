using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankingScene : MonoBehaviour
{
    public Text[] scoreTexts;

    void Awake()
    {
        Ranking.instance.CheckInstance();
        SetRankingScore();
    }

    void SetRankingScore()
    {

        Ranking.instance.scoreList.Sort();
        Ranking.instance.scoreList.Reverse();

        for(int i = 0;i<10;i++)
        {
            scoreTexts[i].text = (i + 1).ToString() + ".";
        }
        for(int i = 0;i<Ranking.instance.scoreList.Count;i++)
        {
            scoreTexts[i].text += " " + Ranking.instance.scoreList[i].ToString();
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            EscapeProcess();
    }

    public void EscapeProcess()
    {
        SceneFader.GetInstance().ChangeScene(0);
    }
}
