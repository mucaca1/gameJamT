using System;
using UnityEngine.ParticleSystemJobs;
using UnityEngine;

public class Collectible : MonoBehaviour 
{
    public static event Action<Player.PlayerTag, Collectible> onCollect;

    public ParticleSystem particles;
    public AudioClip spawn;
    public AudioClip collect;

    private AudioSource audioPlayer;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private bool status;

    void Awake() 
    {
        audioPlayer = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();    
        Despawn();

        GameManager.gameStatus += GameStatus;
    }

    private void GameStatus(bool status)
    {
        this.status = status;
    }

    public void Spawn()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        particles.Play();
        audioPlayer.PlayOneShot(spawn);
    }

    public void Despawn()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log(status);
        Debug.Log(other.name);
        if (status && other.tag == "Player") 
        {
            if (!other.gameObject.GetComponent<Player>().speeding)
            {
                onCollect?.Invoke(other.GetComponent<Player>().playerTag, this);
                Despawn();
                audioPlayer.PlayOneShot(collect);
            }
        }    
    }

    private void OnDestroy() 
    {
        GameManager.gameStatus -= GameStatus;    
    }
}