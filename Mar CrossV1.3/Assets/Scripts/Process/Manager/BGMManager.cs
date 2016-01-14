using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public AudioClip introBgm;
    public AudioClip mainBgm;
    public AudioClip energencyBgm;
    public AudioClip endingBgm;

    AudioSource adSource;


    void Awake()
    {
        adSource = GetComponent<AudioSource>();

    }

    void Update()
    {

    }

    public void StopIntroBGM()
    {
        adSource.Stop();
        adSource.clip = null;

        adSource.clip = mainBgm;
        adSource.loop = true;
        adSource.Play();
    }
    public void OnGameOver()
    {
        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audios.Length;i++)
        {
            audios[i].Stop();
        }
        adSource.Stop();
        adSource.clip = endingBgm;
        adSource.Play();
    }

    public void OnHpEnergency()
    {
        adSource.Stop();
        adSource.clip = energencyBgm;
        adSource.loop = true;
        adSource.Play();
    }
    public void OnHpHealed()
    {
        adSource.Stop();
        adSource.clip = mainBgm;
        adSource.loop = true;
        adSource.Play();
    }
}
