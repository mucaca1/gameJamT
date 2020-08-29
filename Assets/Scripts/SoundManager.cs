using UnityEngine;

public class SoundManager
{
    private AudioClip bckground;

    private bool isPlaying = false;

    private GameObject backgroundMusic;

    private bool soundOn = true;

    public SoundManager()
    {
        // Load default audio from resources
        LoadAudioClipFromFile("horror_ambient_forest", ref bckground);


        soundOn = true;
        SetSoundSetting(soundOn);
    }

    public bool IsSoundIOn()
    {
        return soundOn;
    }

    public void SetSoundSetting(bool isSoundOn)
    {
        soundOn = isSoundOn;

        if (isSoundOn)
            AudioListener.volume = 1f;
        else
            AudioListener.volume = 0f;
    }

    private void LoadAudioClipFromFile(string audioName, ref AudioClip audioClip)
    {
        audioClip = Resources.Load<AudioClip>(audioName);
    }

    public void PlaySound(AudioClip sound)
    {
        if (!soundOn || sound == null)
            return;

        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(sound);

        Object.Destroy(soundGameObject, sound.length);
    }

    public GameObject PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
            Object.Destroy(backgroundMusic);

        GameObject soundGameObject = new GameObject("BackgroundMusic");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = bckground;
        audioSource.Play();
        backgroundMusic = soundGameObject;
        return soundGameObject;
    }
}