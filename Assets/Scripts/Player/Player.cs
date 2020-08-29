using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public Vector2 direction;

    public GameObject wallPrefab;
    public GameObject chargePrefab;

    private static float wallOffset = 1;
    public Animator animator;
    
    // Start is called before the first frame update
    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
