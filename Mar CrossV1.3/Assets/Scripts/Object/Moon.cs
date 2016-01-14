using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour, ITouchable
{
    UIManager uiMgr;

    void Awake()
    {
        uiMgr = GameObject.FindObjectOfType<UIManager>();
    }
	void Update ()
    {
	
	}

    public void OnTouch()
    {
        SceneFader.GetInstance().ChangeScene(1);
    }
}
