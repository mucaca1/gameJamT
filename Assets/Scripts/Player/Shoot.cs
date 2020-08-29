using System.Threading.Tasks;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Sprite bullet1;
    public Sprite bullet2;

    private Vector2 direction;
    private GameObject source;

    public float speed;
    public int life = 2;

    private void Start() 
    {
        gameObject.SetActive(true);
    }

    public void ShootBullet(Vector2 direction, GameObject source, int type)
    {
        gameObject.SetActive(true);

        Invoke("DestroyBullet", life);

        GetComponentInChildren<SpriteRenderer>().sprite = type == 1 ? bullet1 : bullet2;

        Debug.Log(direction);
        this.direction = direction;
        this.source = source;

        transform.Rotate(Vector3.forward, Mathf.Atan2(direction.y * -1f, direction.x * -1f) * Mathf.Rad2Deg);
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