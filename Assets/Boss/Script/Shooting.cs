using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooting : MonoBehaviour
{
    [Header("ShootPoint")]
    public GameObject bullet;
    public float timer;  // ВРЕМЯ НАДО ПЕРЕДАВАТЬ ОТ УДАРА

    void Update()
    {
        StartCoroutine(ShootOne());
        if (timer <= 0)
        {
            Boss boss = FindObjectOfType<Boss>();
            boss.activState = Boss.BossState.MOVE;


            gameObject.SetActive(false);
            timer = 7;
        }
    }



    IEnumerator ShootOne()
    {
        timer -= Time.deltaTime;

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject throwable = Instantiate(bullet, new Vector2(Random.Range(0, 30), transform.position.y), Quaternion.identity);
            throwable.GetComponent<ElectricBullet>().direction = -transform.up;

            StopAllCoroutines();
        }
    }

}




