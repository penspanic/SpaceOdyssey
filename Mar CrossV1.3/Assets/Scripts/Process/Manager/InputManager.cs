using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    Tutorial tutorial;
    void Awake()
    {
        tutorial = GameObject.FindObjectOfType<Tutorial>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.timeScale == 1 && tutorial.isTutorialEnd)
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(wp, Vector2.zero);

            for(int i = 0;i<hitInfo.Length;i++)
            {
                if(hitInfo[i].transform.GetComponent<ITouchable>() != null)
                {
                    hitInfo[i].transform.GetComponent<ITouchable>().OnTouch();
                }
            }
        }
    }

}
