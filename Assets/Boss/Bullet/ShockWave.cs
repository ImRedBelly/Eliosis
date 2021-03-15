using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector2 direction;

    void Update()
    {
        Fly();
    }
    public void Fly()
    {
        transform.Translate(direction * 8 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
