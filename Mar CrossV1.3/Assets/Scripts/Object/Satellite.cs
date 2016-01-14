using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour, ITouchable
{
    public float moveSpeed;
    public float moveDistance;

    float riseEndY;
    float fallEndY;

    void Awake()
    {
        riseEndY = transform.position.y + moveDistance / 2;
        fallEndY = transform.position.y - moveDistance / 2;
    }
    public void OnTouch()
    {
        SceneFader.GetInstance().ChangeScene(2);
    }

    bool isRising = true;
    void Update()
    {
        if (isRising)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y >= riseEndY)
                isRising = false;
        }
        else
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime, Space.World);
            if (transform.position.y <= fallEndY)
                isRising = true;
        }
    }
}
