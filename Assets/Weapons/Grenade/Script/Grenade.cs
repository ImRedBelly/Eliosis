using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    ValueManagerPlayer valuePlayer;

    public RTrajectotyTwo trajectoryRenderer;




    public GameObject grenadePrefab;
    public GameObject effectBoom;
    public float power = 50;
    public float damageValue = 5;
    public GameObject cam;

    GameObject grenade;
    private void Start()
    {
        valuePlayer = ValueManagerPlayer.instance;
    }
    void Update()
    {
        float enter;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
        Vector3 mouseInWorld = ray.GetPoint(enter);

        Vector3 speed = (mouseInWorld - transform.position);



        if (Input.GetKey(KeyCode.G))
        {
            trajectoryRenderer.gameObject.SetActive(true);
            trajectoryRenderer.ShowTrajectory(transform.position, speed);
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            trajectoryRenderer.gameObject.SetActive(false);
            grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            grenade.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);
            trajectoryRenderer.AddBody(grenade.GetComponent<Rigidbody2D>());
            StartCoroutine(IsTrigger(grenade.gameObject));
            StartCoroutine(Boom());
        }
    }
    void DoDashDamage()
    {
        damageValue = Mathf.Abs(damageValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(grenade.transform.position, 0.9f);
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
        trajectoryRenderer.RemoveBody(grenade.GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(2f);
        DoDashDamage();
        Instantiate(effectBoom, grenade.transform.position, Quaternion.identity);
        Destroy(grenade);
    }
    IEnumerator IsTrigger(GameObject grenade)
    {
        yield return new WaitForSeconds(0.1f);
        grenade.GetComponent<CapsuleCollider2D>().isTrigger = false;
    }
}
