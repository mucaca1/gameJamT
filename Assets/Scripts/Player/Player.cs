using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float health;
    public Vector2 direction;

    public GameObject wallPrefab;
    public GameObject chargePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        GameObject a = Instantiate(chargePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        a.getComponent<Shoot>().direction = this.direction;
    }

    void MakeWall()
    {
        
    }

}
