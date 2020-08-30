using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.ParticleSystemJobs;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{

    public static event Action<PlayerTag, int> onHit;
    public static event Action<PlayerTag> onDeath;
    public static event Action<Player> onSpawn;

    private Vector3 initPosition;

    public enum PlayerTag : int
    {
        One = 1,
        Two = 2
    }

    public AnimatorController[] animations;

    public PlayerTag playerTag = PlayerTag.One;

    public float moveSpeed;
    public float health;
    private Vector2 direction;
    public int deadTimer = 3;

    public GameObject wallPrefab;
    public GameObject chargePrefab;

    private static float wallOffset = 1;

    private Animator animator;
    private InputManager inputManager;

    public ParticleSystem particles;
    public Color hitParticlesColor;

    public AudioClip[] attack;
    public AudioClip hit;

    private Material material;
    public GameObject splashPrefab;

    private bool speeding = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();

        animator.runtimeAnimatorController = animations[((int)playerTag) - 1];

        if (playerTag == PlayerTag.One) {
            inputManager.inputAlternative = InputManager.InputAlternative.One;
        }
        else {
            inputManager.inputAlternative = InputManager.InputAlternative.Two;
        }

        direction = playerTag == PlayerTag.One ? Vector2.right : Vector2.left;

        initPosition = this.transform.position;

        particles.startColor = hitParticlesColor;
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.GetJoystickInput().x != 0 || inputManager.GetJoystickInput().y != 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("xAxisMove", inputManager.GetJoystickInput().x);
            animator.SetFloat("yAxisMove", inputManager.GetJoystickInput().y);

            gameObject.transform.position += new Vector3(
                inputManager.GetJoystickInput().x,
                inputManager.GetJoystickInput().y * -1f,
                0.0f
            ).normalized * (speeding ? (moveSpeed * 3) : moveSpeed) * Time.deltaTime;

            Debug.Log("Player " + inputManager.inputAlternative + ": horizontal - [" + inputManager.GetJoystickInput().x + "], vertical - [" + inputManager.GetJoystickInput().y * -1 + "]");
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        
        direction = inputManager.GetJoystickInput();
        direction.y *= -1f;

        if (inputManager.GetFireButton())
        {
            SoundPlayer.PlaySound(attack[Random.Range(0,attack.Length)]);
            if (direction.normalized.magnitude != 0)
                Attack();
            else
                SplashDisplay();
            
            Debug.Log("Player " + inputManager.inputAlternative + ": Fire button");
        }

        if (inputManager.GetActionButton())
        {
            Debug.Log("Player " + inputManager.inputAlternative + ": Action button");
        }

        speeding = inputManager.GetActionButton();

        if (inputManager.GetMenuButton())
        {
            Debug.Log("Player " + inputManager.inputAlternative + ": Menu button");
        }
        
    }

    public void Attack()
    {
        //animator.SetBool("isShooting", true);
        GameObject a = Instantiate(chargePrefab, this.transform.position, Quaternion.identity);
        Shoot bullet = a.GetComponent<Shoot>();
        bullet.ShootBullet(direction, gameObject, (int)playerTag);
    }

    public void Hit(int damage) 
    {
        Debug.Log("Taken damage :" + damage);

        SoundPlayer.PlaySound(hit);
        
        health -= damage;

        onHit?.Invoke(playerTag, (int)health);

        particles.Play();

        material.SetFloat("_IsBlinking", 1);
        Invoke("EndBlinking", 2);

        if (health <= 0)
            Die();
    }

    private void EndBlinking() 
    {
        material.SetFloat("_IsBlinking", 0);
    }

    public void Respawn()
    {
        health = 3;
        this.transform.position = initPosition;
        animator.SetBool("isDead", false);
        inputManager.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;

        onSpawn?.Invoke(this);
    }

    // Omae wa mou shindeiru
    // https://www.youtube.com/watch?v=dNQs_Bef_V8
    private void Die() 
    {
        EndBlinking();
        onDeath?.Invoke(playerTag);
        animator.SetBool("isDead", true);
        inputManager.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        Invoke("Respawn", deadTimer);
    }

    private void SplashDisplay()
    {
        GameObject a = Instantiate(splashPrefab, this.transform.position, Quaternion.identity);
        Destroy(a, 1.6f);
    }
}
