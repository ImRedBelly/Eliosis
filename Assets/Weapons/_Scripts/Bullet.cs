using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    ValueManagerPlayer valuePlayer;
    ValueManagerEnemy valueEnemy;

    [HideInInspector]
    public Vector2 direction;

    public GameObject sandPrefab;
    public GameObject platformPrefab;
    public GameObject bloodPrefab;

    Rigidbody2D rb;
    private void Awake()
    {
        valuePlayer = ValueManagerPlayer.instance;
        valueEnemy = ValueManagerEnemy.instance;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = Quaternion.AngleAxis(Random.Range(-5, 5), transform.forward) * direction * (valuePlayer.bulletValue.speed * Random.Range(0.75f, 1.25f));
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

        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject platform = Instantiate(bloodPrefab, transform.position, transform.rotation);
            collision.gameObject.GetComponent<HealthPlayer>().ApplyDamage(valueEnemy.bulletValue.damageBullet, transform.position);
            Destroy(platform, 1f);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject platform = Instantiate(bloodPrefab, transform.position, transform.rotation);
            collision.gameObject.SendMessage("ApplyDamage", valuePlayer.bulletValue.damageBullet);
            Destroy(platform, 1f);
        }

        Destroy(gameObject);
    }



    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
