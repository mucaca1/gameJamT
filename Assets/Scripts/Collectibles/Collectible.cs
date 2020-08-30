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

    void Awake() 
    {
        audioPlayer = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();    
    }

    void Start() 
    {
        Despawn();
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
        if (other.tag == "Player") 
        {
            onCollect?.Invoke(other.GetComponent<Player>().playerTag, this);
            Despawn();
            audioPlayer.PlayOneShot(collect);
        }    
    }
}