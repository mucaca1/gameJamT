using UnityEngine;

public static class SoundPlayer
{
    public static void PlaySound(AudioClip sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(sound);
        
        Object.Destroy(soundGameObject, sound.length);
    }
}