using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraSizeModifier : MonoBehaviour
{
    public int DesiredWidth = 1280;
    public int DesiredHeight = 720;
    public float scaler = 1.0f;
    Camera cam;

    public bool isProcessing = true;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    
    void Update()
    {
        if (!isProcessing)
            return;
        int width = Screen.width;
        int height = Screen.height;
        float desiredRatio = (float)DesiredWidth / (float)DesiredHeight;
        float curRatio = (float)width / (float)height;

        if (curRatio < desiredRatio)
        {
            cam.orthographicSize = 1.4f * (1.0f / curRatio) * scaler;
        }
        else
        {
            cam.orthographicSize = 1.4f;
        }
    }
}
