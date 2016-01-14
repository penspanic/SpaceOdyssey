using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    bool doShake;
    float shakeTime;
    float shakeAmount;
    float shakeTimeCount;
    Vector3 currentVelocity;
    //float smoothTime = 0.3f;

    bool endShake;

    public Transform cameraPosition;

    void Update()
    {
        if (endShake)
        {
            transform.position = cameraPosition.position;
            endShake = false;
        }

        if (doShake == false)
            return;

        Vector3 rv = Random.insideUnitSphere;
        Vector3 rvXZ = new Vector3(rv.x, 0, rv.z);
        transform.position = transform.position+rvXZ * shakeAmount;

        shakeTimeCount += Time.deltaTime;
        if (shakeTimeCount > shakeTime)
        {
            doShake = false;
            shakeTimeCount = 0;
            endShake = true;
        }
    }


    public void ShakeCamera(float time, float amount)
    {
        shakeTime = time;
        shakeAmount = amount;
        doShake = true;
    }
}
