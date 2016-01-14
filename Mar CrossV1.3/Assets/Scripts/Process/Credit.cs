using UnityEngine;
using System.Collections;

public class Credit : MonoBehaviour {


    public void OnReturn()
    {
        SceneFader.GetInstance().ChangeScene(0);
    }
}
