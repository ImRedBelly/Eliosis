using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector2 direction;
    Rigidbody2D rb;

    private void Start()
    {
        //Destroy(gameObject, 10);
        rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        Fly();
    }
    public void Fly()
    {
        //transform.Translate(direction * 8 * Time.deltaTime);
        rb.velocity = (PlayerMovement.instance.transform.position - transform.position) * 4; // хрен увернешся

    }
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
