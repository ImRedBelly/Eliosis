using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector2 direction;
    private void Start()
    {

        Destroy(gameObject, 10);
    }
    void Update()
    {
        Fly();
    }
    public void Fly()
    {
        transform.Translate(direction * 8 * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Shock Player");
            collision.gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
            Destroy(gameObject);
        }
        else
        {
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
