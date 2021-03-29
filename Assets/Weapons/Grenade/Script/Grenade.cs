using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Rigidbody2D rb;
    ValueManagerPlayer valuePlayer;
    public GameObject effectBoom;
    public float damageValue = 5;
    public float power = 50;


    public Camera cam;
    AudioSource audioSource;
    public AudioClip boom;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        cam = Camera.main;

        valuePlayer = ValueManagerPlayer.instance;
        TrajectoryRenderer.instance.AddBody(rb);


        StartCoroutine(Boom());
        StartCoroutine(IsTrigger(gameObject));
    }



    void DoDashDamage()
    {
        damageValue = Mathf.Abs(damageValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(transform.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy" || collidersEnemies[i].gameObject.tag == "DeathCopy")
            {
                if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                {
                    damageValue = -damageValue;
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", valuePlayer.bulletValue.damageBullet * 3);
                cam.GetComponent<CameraFollow>().ShakeCamera();
            }
        }
    }
    IEnumerator Boom()
    {
        TrajectoryRenderer.instance.RemoveBody(rb);
        yield return new WaitForSeconds(2f);
        audioSource.PlayOneShot(boom);
        DoDashDamage();
        Instantiate(effectBoom, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator IsTrigger(GameObject grenade)
    {
        yield return new WaitForSeconds(0.1f);
        grenade.GetComponent<CircleCollider2D>().isTrigger = false;
    }
}
