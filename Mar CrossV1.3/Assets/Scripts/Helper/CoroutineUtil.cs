using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    // 다만들면 + 브레이킹 ㄱㄱ
    public static IEnumerator MoveObj(GameObject obj, Vector2 startPos, Vector2 endPos, float time, bool isBreakEffect)
    {

        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            if (elapsedTime >= time)
                elapsedTime = time;

            elapsedTime += Time.unscaledDeltaTime;

            Vector2 temp;
            temp = startPos + (endPos - startPos) * (elapsedTime / time);
            obj.transform.position = temp;

            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator MoveUIImg(Image obj, Vector2 startLocalPos, Vector2 endLocalPos, float time, bool isBreakEffect)
    {

        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            if (elapsedTime >= time)
                elapsedTime = time;

            elapsedTime += Time.deltaTime;

            Vector2 temp;
            temp = startLocalPos + (endLocalPos - startLocalPos) * (elapsedTime / time);
            obj.rectTransform.localPosition = temp;

            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator LerpScale(GameObject go, Vector3 startScale, Vector3 endScale, float speed)
    {
        yield return null;
        float elapseTime = 0.0f;
        Vector3 changeScale;
        Vector3 prevScale;
        while (true)
        {
            elapseTime += Time.deltaTime * speed;

            prevScale = go.transform.localScale;
            go.transform.localScale = Vector3.Lerp(startScale, endScale, elapseTime);
            changeScale = go.transform.localScale - prevScale;

            if (changeScale.magnitude < 0.001f)
            {
                go.transform.localScale = endScale;
                break;
            }
            yield return null;
        }
        
    }
    public static IEnumerator LerpColor(SpriteRenderer renderer, Color startColor, Color endColor, float time, bool isScaled = true)
    {
        float elapsedTime = 0f;

        while(elapsedTime < time)
        {
            if(isScaled)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime += Time.unscaledDeltaTime;
            }

            Color currColor = Color.Lerp(renderer.color, endColor, elapsedTime);
            renderer.color = currColor;

            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator LerpMove(GameObject go, Vector3 startPos, Vector3 endPos, float speed, bool isScaled = true, bool isLocalMove = false, GameObject messageObj = null, string methodName = null)
    {
        yield return new WaitForEndOfFrame();
        float elapseTime = 0.0f;

        Vector3 movedDistance;  // 이전에 이동한 거리
        Vector3 prevPos;
        while (true)
        {
            if (isScaled)
            {
                elapseTime += Time.deltaTime * speed;
            }
            else
            {
                elapseTime += Time.unscaledDeltaTime * speed;
            }
            if (isLocalMove)
            {
                prevPos = go.transform.localPosition;
                go.transform.localPosition = Vector3.Lerp(startPos, endPos, elapseTime);
                movedDistance = go.transform.localPosition - prevPos;
            }
            else
            {
                prevPos = go.transform.position;
                go.transform.position = Vector3.Lerp(startPos, endPos, elapseTime);
                movedDistance = go.transform.position - prevPos;
            }

            if (movedDistance.magnitude < 0.001f)
            {
                if (isLocalMove)
                    go.transform.localPosition = endPos;
                else
                    go.transform.position = endPos;
                break;
            }
            yield return new WaitForEndOfFrame();

        }
        if (messageObj != null)
        {
            messageObj.SendMessage(methodName);
        }
    }
    public static IEnumerator RotateObj(GameObject obj, Vector3 startRot, Vector3 endRot, float time, bool isScaled = true, bool isLocalRotate = false, GameObject messageObj = null, string methodName = null)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < time)
        {
            if (elapsedTime >= time)
                elapsedTime = time;
            if (isScaled)
                elapsedTime += Time.deltaTime;
            else
                elapsedTime += Time.unscaledDeltaTime;

            Vector2 temp;
            temp = startRot + (endRot - startRot) * (elapsedTime / time);
            obj.transform.rotation = Quaternion.Euler(temp);

            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator RotateUIObj(GameObject obj, Vector3 startRot, Vector3 endRot, float time, bool isScaled = true, bool isLocalRotate = false, GameObject messageObj = null, string methodName = null)
    {
        float elapsedTime = 0.0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        while (elapsedTime < time)
        {
            if (elapsedTime >= time)
                elapsedTime = time;
            if (isScaled)
                elapsedTime += Time.deltaTime;
            else
                elapsedTime += Time.unscaledDeltaTime;

            Vector2 temp;
            temp = startRot + (endRot - startRot) * (elapsedTime / time);
            rectTransform.rotation = Quaternion.Euler(temp);

            yield return new WaitForEndOfFrame();
        }
    }
}
