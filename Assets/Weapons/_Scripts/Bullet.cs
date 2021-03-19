using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    public float speed;

    public GameObject sandPrefab;
    public GameObject platformPrefab;
    public GameObject bloodPrefab;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = Quaternion.AngleAxis(Random.Range(-5, 5), transform.forward) * direction * (speed * Random.Range(0.75f, 1.25f));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Sand"))
        {
            GameObject sand = Instantiate(sandPrefab, transform.position, transform.rotation);
            Destroy(sand, 1f);

            return;
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            GameObject blood = Instantiate(platformPrefab, transform.position, transform.rotation);
            Destroy(blood, 1f);

            return;
        }


        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))

        {
            GameObject platform = Instantiate(bloodPrefab, transform.position, transform.rotation);
            Destroy(platform, 1f);

            print("попал");
            //collision.gameObject.SendMessage("ApplyDamage", Mathf.Sign(direction.x) * 2f);
        }

        Destroy(gameObject);



    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
