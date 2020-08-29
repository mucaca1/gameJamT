using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public GameObject source;

    private void Update()
    {
        this.transform.position += new Vector3(direction.x + (speed * Time.deltaTime), direction.y + (speed * Time.deltaTime), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && other.gameObject != source) 
        {
            Player player = other.GetComponent<Player>();
            player.Hit(1);
        }
    }
}