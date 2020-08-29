using System.Threading.Tasks;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public GameObject source;
    public int life = 2;

    private void Start() 
    {
        Invoke("DestroyBullet", 2);
    }

    private void Update()
    {
        this.transform.position += new Vector3(direction.x * (speed * Time.deltaTime), direction.y * (speed * Time.deltaTime), 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject == source)
            return;

        if (other.tag == "Player") 
        {
            Player player = other.GetComponent<Player>();
            player.Hit(1);
        }

        DestroyBullet();
    }
    
    private void DestroyBullet() 
    {
        Destroy(gameObject);
    }
}