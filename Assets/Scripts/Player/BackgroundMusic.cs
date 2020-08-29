using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class BackgroundMusic : MonoBehaviour
{
    public int musicIndex;
    private GameObject backgroundMusic;

    private AudioClip gameBackground;
    private AudioClip menuBackground;
    private void Start()
    {
        LoadAudioClipFromFile("Audio/Background", ref gameBackground);
        LoadAudioClipFromFile("Audio/Menu", ref menuBackground);
        PlayBackgroundMusic(musicIndex);
    }

    public GameObject PlayBackgroundMusic(int index) {
        if (backgroundMusic != null)
            Object.Destroy(backgroundMusic);
        
        GameObject soundGameObject = new GameObject("BackgroundMusic");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        switch (index)
        {
            case 0:
                audioSource.clip = menuBackground;
                break;
            case 1:
                audioSource.clip = gameBackground;
                break;
        }
        audioSource.Play();
        backgroundMusic = soundGameObject;
        return soundGameObject;
    }

    public void StopPlayBackdround()
    {
        if (backgroundMusic != null)
            Object.Destroy(backgroundMusic);
    }
    
    private void LoadAudioClipFromFile(string audioName, ref AudioClip audioClip) {
        audioClip = Resources.Load<AudioClip>(audioName);
    }
}