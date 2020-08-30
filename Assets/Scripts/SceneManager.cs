using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public AudioClip pass;

    void Start() 
    {
        GameData.GetInstance();    
    }

    private void Update()
    {
        bool actionButton = Input.GetKeyDown("joystick 1 button 0") || Input.GetKeyDown("e");
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "GamePlay" && actionButton)
        {
            SoundPlayer.PlaySound(pass);
            Debug.Log("Load scene");
            PlayScene();
        }
    }

    public void PlayScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
}