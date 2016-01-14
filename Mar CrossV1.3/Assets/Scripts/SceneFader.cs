using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFader: MonoBehaviour {

	float fadeSpeed = 7f;

	public Transform uiCanvas;
	public Transform black;
	public Transform fadeObject;

    public Image fadeObjectImage;

    public bool isFading = true;
    static SceneFader sceneFader;
    public static SceneFader GetInstance()
    {
        return sceneFader;
    }

	void Awake(){
        sceneFader = this;

		fadeObject = Instantiate (black);
		fadeObject.SetParent (uiCanvas);
		fadeObject.localPosition = new Vector3 (0, 0, 0);
		fadeObject.localScale = new Vector3 (1, 1, 1);
        fadeObjectImage = fadeObject.GetComponent<Image>();
		FadeIn ();

	}

    void Update()
    {
            
    }

	public void FadeIn(){
		Time.timeScale = 0;
        isFading = true;
		StartCoroutine (Fading(Color.clear));
	}

	public void FadeOut(){
		Time.timeScale = 0;
        isFading = true;
		StartCoroutine (Fading(Color.black));
	}
	
	IEnumerator Fading(Color targetColor){
        fadeObject.gameObject.SetActive(true);
        while (Mathf.Abs(fadeObjectImage.color.a - targetColor.a) >= 0.05f)
        {
			yield return null;
            fadeObjectImage.color = Color.Lerp(fadeObjectImage.color, targetColor, fadeSpeed * 0.016f);
		}
        fadeObjectImage.color = targetColor;
		Time.timeScale = 1;
        if(targetColor == Color.clear)
            fadeObject.gameObject.SetActive(false);
        isFading = false;
	}

    public void ChangeScene(int level)
    {
        StartCoroutine(WaitForFadeOut(level));
    }

    IEnumerator WaitForFadeOut(int level)
    {
        yield return StartCoroutine(Fading(Color.black));
        fadeObject.gameObject.SetActive(true);
        Application.LoadLevel(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}