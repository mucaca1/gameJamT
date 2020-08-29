using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

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

            gameObject.transform.position += new Vector3(
                inputManager.GetJoystickInput().x,
                inputManager.GetJoystickInput().y * -1f,
                0.0f
            ) * moveSpeed * Time.deltaTime;
            
            
            Debug.Log("Player " + inputManager.inputAlternative + ": horizontal - [" + inputManager.GetJoystickInput().x + ", vertical - " + inputManager.GetJoystickInput().y * -1 + "]");
        }
        else
        {
            this.animator.SetBool("isMoving", false);
        }

        if (inputManager.GetFireButton())
        {
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
        a.GetComponent<Shoot>().direction = this.direction;
    }

    public void MakeWall()
    {
        GameObject a = Instantiate(wallPrefab, this.transform.position + new Vector3(direction.x * wallOffset, direction.y * wallOffset, 0.0f), Quaternion.identity);
        a.GetComponent<Shoot>().direction = this.direction;
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
