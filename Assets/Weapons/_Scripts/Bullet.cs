using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public float speed;
	public int damage = -25;
	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		rb.velocity = Quaternion.AngleAxis(Random.Range(-5, 5), transform.forward) * transform.right * (PlayerMovement.instance.transform.localScale.x * 5) * speed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))

		{
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
