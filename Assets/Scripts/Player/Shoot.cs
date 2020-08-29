using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    private void Update()
    {
        this.transform.position += new Vector3(direction.x + (speed * Time.deltaTime), direction.y + (speed * Time.deltaTime), 0.0f);
    }
}