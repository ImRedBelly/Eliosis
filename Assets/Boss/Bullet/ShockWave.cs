using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector2 direction;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        Destroy(gameObject, 10);
        rb.velocity = (PlayerMovement.instance.transform.position - transform.position); // хрен увернешся

    }

    //public void Fly()
    //{
    //    transform.Translate(direction * 8 * Time.deltaTime);

    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Shock Player");
            collision.gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            print("Shock Respawn");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
