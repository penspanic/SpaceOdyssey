using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour ,ITouchable
{
    GameManager gameMgr;
    void Awake()
    {
        gameMgr = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {


    }
    
    public void OnTouch()
    {
        gameMgr.OnGameStart();
    }
}
