using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public Vector2 direction;

    public GameObject wallPrefab;
    public GameObject chargePrefab;

    private static float wallOffset = 1;

    public Animator animator;
    public InputManager inputManager;

    public AudioClip[] attack;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inputManager.GetJoystickInput().x != 0 || inputManager.GetJoystickInput().y != 0)
        {
            //this.animator.SetBool("isMoving", true);
            this.animator.SetBool("isMoving", true);
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
            this.animator.SetBool("isMoving", false);
        }

        if (inputManager.GetFireButton())
        {
            SoundPlayer.PlaySound(attack[Random.Range(0,attack.Length)]);
            Attack();
            Debug.Log("Player " + inputManager.inputAlternative + ": Fire button");
        }

        if (inputManager.GetActionButton())
        {
            MakeWall();
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
        Debug.Log("Shoot dir: " + direction);
        Shoot bullet = a.GetComponent<Shoot>();
        bullet.direction = direction;
        bullet.source = gameObject;
    }

    public void MakeWall()
    {
        GameObject a = Instantiate(wallPrefab, this.transform.position + new Vector3(direction.x * wallOffset, direction.y * wallOffset, 0.0f), Quaternion.identity);
        a.GetComponent<Shoot>().direction = this.direction;
    }

    public void Hit(int damage) 
    {
        health -= damage;
        Debug.Log("Taken damage :" + damage);
        animator.SetBool("isDead", health <= 0);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Shoot")
        {
            // zasiahnutý
            health--;
            if (health <= 0)
            {
                // Skap!
                animator.SetBool("isDead", true);
            }
        }
    }
}
