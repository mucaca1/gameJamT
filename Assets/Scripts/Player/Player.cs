using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{

    public static event Action<PlayerTag, int> onHit;
    public static event Action<PlayerTag> onDeath;

    public enum PlayerTag : int
    {
        One = 1,
        Two = 2
    }

    public AnimatorController[] animations;

    public PlayerTag playerTag = PlayerTag.One;

    public float moveSpeed;
    public float health;
    public Vector2 direction;

    public GameObject wallPrefab;
    public GameObject chargePrefab;

    private static float wallOffset = 1;

    public Animator animator;
    public InputManager inputManager;

    public AudioClip[] attack;
    public AudioClip hit;
    
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
            ) * moveSpeed * Time.deltaTime;
            
            direction = inputManager.GetJoystickInput();
            direction.y *= -1f;
            
            Debug.Log("Player " + inputManager.inputAlternative + ": horizontal - [" + inputManager.GetJoystickInput().x + "], vertical - [" + inputManager.GetJoystickInput().y * -1 + "]");
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (inputManager.GetFireButton())
        {
            SoundPlayer.PlaySound(attack[Random.Range(0,attack.Length)]);
            Attack();
            Debug.Log("Player " + inputManager.inputAlternative + ": Fire button");
        }

        if (inputManager.GetActionButton())
        {
            Debug.Log("Player " + inputManager.inputAlternative + ": Action button");
        }

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

        if (health <= 0) 
            Die();
    }

    private void Die() 
    {
        onDeath?.Invoke(playerTag);
        animator.SetBool("isDead", true);
        inputManager.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
