using UnityEngine;

public class ElectricBullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 10;


    public string enemyMask;
    public string playerMask;

    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer(enemyMask);
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            gameObject.layer = LayerMask.NameToLayer(playerMask);
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
