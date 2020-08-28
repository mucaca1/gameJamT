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
        float actionButton = Input.GetAxis("Fire" + playerTag);
        float fireButton = Input.GetAxis("Action" + playerTag);
        float menu = Input.GetAxis("Menu" + playerTag);
        if (horizontal != 0 || vertical != 0)
        {
            gameObject.transform.position += new Vector3(horizontal * moveSpeed, vertical * moveSpeed * -1f, 0.0f);
            direction = new Vector2(horizontal, vertical * -1f);
        }

        if (fireButton != 0 || actionButton != 0)
        {
            Debug.Log("Player " + playerTag + " - Fire " + fireButton + " | action " + actionButton);
        }

        if (menu != 0)
        {
            Debug.Log("Player " + playerTag + " - Menu " + menu);
        }
    }
}