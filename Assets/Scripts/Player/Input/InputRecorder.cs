using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : Player
{
    public string playerTag;


    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical" + playerTag);
        float horizontal = Input.GetAxis("Horizontal" + playerTag);
        bool actionButton = Input.GetKeyDown("joystick " + playerTag + " button 0");
        bool fireButton = Input.GetKeyDown("joystick " + playerTag + " button 1");
        bool menu = Input.GetKeyDown("joystick " + playerTag + " button 6");
        
        if (horizontal != 0 || vertical != 0)
        {
            gameObject.transform.position += new Vector3(horizontal * moveSpeed * Time.deltaTime, vertical * moveSpeed * -1f * Time.deltaTime, 0.0f);
            direction = new Vector2(horizontal, vertical * -1f);
            
            if (direction.x != 0 && direction.y != 0)
                direction.y = 0;
            
            Debug.Log("Player " + playerTag + ": horizontal - [" + horizontal + ", vertical - " + vertical * -1 + "]");
        }

        if (fireButton)
        {
            Attack();
            Debug.Log("Player " + playerTag + ": Fire button");
        }

        if (actionButton)
        {
            MakeWall();
            Debug.Log("Player " + playerTag + ": Action button");
        }
        if (menu)
        {
            Debug.Log("Player " + playerTag + ": Menu button");
        }
    }
}