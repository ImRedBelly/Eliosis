using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCopyEnemy : MonoBehaviour
{
    public SpriteRenderer spriteHead;
    public SpriteRenderer spriteBody;
    public Transform[] bodyParts;
    public GameObject deadEffect;
    void Start()
    {

        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(5, 10), ForceMode2D.Impulse);
            bodyParts[i].GetComponent<Rigidbody2D>().AddTorque(Random.Range(-50, 50), ForceMode2D.Force);
        }
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < bodyParts.Length; i++)
        {
            Instantiate(deadEffect, bodyParts[i].transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.1f);
        //for (int i = 0; i < bodyParts.Length; i++)
        //{
        //    Rigidbody2D rb = bodyParts[i].GetComponent<Rigidbody2D>();
        //    TrajectoryRenderer.instance.RemoveBody(rb);
        //}
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

