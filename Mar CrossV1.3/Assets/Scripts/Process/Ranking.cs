using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ranking : MonoBehaviour
{

    static Ranking _instance;

    public static Ranking instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject newObj = new GameObject();
                newObj.name = "Ranking";
                newObj.AddComponent<Ranking>();
                DontDestroyOnLoad(newObj);
                _instance = newObj.GetComponent<Ranking>();
            }
            return _instance;
        }
    }

    const float scoreCapacity = 10;

    public List<int> scoreList = new List<int>();

    public void CheckInstance()
    {

    }

    void Awake()
    {
       
        for (int i = 0; i < 10; i++)
        {
            if (DataLoadSave.HasKey("Score " + i.ToString()))
            {
                scoreList.Add(DataLoadSave.GetInt("Score " + i.ToString()));
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

    }

    public void NewScore(int score)
    {
        scoreList.Sort();

        if (scoreList.Count == 10)
        {
            for (int i = 0; i < scoreList.Count; i++)
            {
                if(scoreList[i]<score)
                {
                    scoreList[i] = score;
                    break;
                }
            }
        }
        else
            scoreList.Add(score);

        scoreList.Sort();
        scoreList.Reverse();

        string scores = string.Empty;
        for(int i = 0;i<scoreList.Count;i++)
        {
            scores += scoreList[i] + " ";
        }
        Debug.Log(scores);
        SaveScoreData();
    }

    void SaveScoreData()
    {
        for(int i = 0;i<scoreList.Count;i++)
        {
            DataLoadSave.SetInt("Score " + i.ToString(), scoreList[i]);
        }
    }
}
