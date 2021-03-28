﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Sniper Options")]
    public bool isSniper;
    bool isShotSniper = true;
    public float[] patrolAngles;
    public GameObject rootSniper;
    public LayerMask player;

    [Header("Components")]
    public Animator animator;
    public Rigidbody2D Rigidbody2D;
    public Animator animatorWeapon;
    public AudioSource audioSource;
    public AudioSource audioSourceWeapon;
    public EnemyWeapon enemyWeapon;

    public LineRenderer lineRenderer;

    [Header("Options")]
    public float life = 5;
    public float speed = 5f;

    private bool facingRight = true;

    [Header("Attack")]
    public Transform attackCheck;
    public Transform placeFire;
    public Transform lazerSocket;

    [Header("Audio")]
    public AudioClip sniperShot;
    public AudioClip dead;


    public GameObject enemy;

    private float distToPlayer;

    public float dmgValue = 2;
    public float meleeDist = 1.5f;
    public float shootDist = 10f;
    public float snipeDist = 15f;

    private bool meleeAttack = false;            //проверка ножа и пушки

    public BossState activState;
    public enum BossState
    {
        IDLE,
        MELEEATTACK,
        SHOOT
    }

    [Header("Dead Effect")]
    public SpriteRenderer[] spriteForDeadCopy;
    public GameObject deadCopy;

    float time = 0;
    private void Start()
    {
        activState = BossState.IDLE;
    }

    void FixedUpdate()
    {
        if (life <= 0)
            DestroyEnemy();


        else if (enemy != null)
        {
            distToPlayer = enemy.transform.position.x - transform.position.x;

            if (isSniper)
            {
                Idle();
                ShotSniper();
            }
            else
            {
                switch (activState)
                {
                    case BossState.IDLE:
                        Idle();
                        meleeAttack = false;

                        if (Mathf.Abs(distToPlayer) < shootDist)
                            activState = BossState.SHOOT;
                        break;

                    case BossState.SHOOT:
                        Run(distToPlayer);

                        if (Mathf.Abs(distToPlayer) > shootDist)
                            activState = BossState.IDLE;

                        if (Mathf.Abs(distToPlayer) > 0 && Mathf.Abs(distToPlayer) < 2)
                            activState = BossState.MELEEATTACK;

                        if (!meleeAttack)
                        {
                            enemyWeapon.SetShotgun();
                            enemyWeapon.CheckFire();
                        }
                        else
                        {
                            enemyWeapon.SetKnife();
                        }
                        break;

                    case BossState.MELEEATTACK:

                        meleeAttack = true;
                        if (Mathf.Abs(distToPlayer) > 3.5f)
                            activState = BossState.SHOOT;
                        else
                            Idle();

                        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Rigidbody2D.velocity.y);
                        if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                            Flip();

                        enemyWeapon.SetKnife();
                        enemyWeapon.CheckFire();
                        break;
                }
            }
        }
        else
        {
            enemy = GameObject.Find("Player _Yura");
        }



        if (transform.localScale.x * Rigidbody2D.velocity.x > 0 && !facingRight && life > 0)
        {
            Flip();
        }
        else if (transform.localScale.x * Rigidbody2D.velocity.x < 0 && facingRight && life > 0)
        {
            Flip();
        }
    }

    void Flip()
    {

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void ApplyDamage(float damage)
    {
        print("Dead");
        damage = Mathf.Abs(damage);
        animator.SetBool("Hit", true);
        life -= damage;
    }


    public void MeleeAttack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].gameObject.tag == "Player")
                player[i].gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
        }
    }

    public void Run(float position)
    {
        animator.SetBool("IsWaiting", false);
        animatorWeapon.SetBool("IsWaiting", false);
        Rigidbody2D.velocity = new Vector2(position / Mathf.Abs(position) * speed, Rigidbody2D.velocity.y);
    }

    public void Idle()
    {
        animator.SetBool("IsWaiting", true);
        animatorWeapon.SetBool("IsWaiting", true);
        Rigidbody2D.velocity = new Vector2(0f, Rigidbody2D.velocity.y);
    }


    void ShotSniper()
    {
        var directionToEndPoint = lazerSocket.position - lazerSocket.right * transform.localScale.x * -50; // конечная точка луча если не видит игрока
        var directionToPlayer = lazerSocket.transform.position - directionToEndPoint; //расстояние RayCast до игрока

        RaycastHit2D hit = Physics2D.Raycast(lazerSocket.position, lazerSocket.right * transform.localScale.x * 4, directionToPlayer.magnitude, player);

        if (hit.collider != null)
        {
            if (isShotSniper)
                StartCoroutine(TimerShotSniper());
            else    // нацеливает луч на игрока если тот попадает на луч
            {
                Vector2 directionToPlayerOffLerp = enemy.transform.position - lazerSocket.transform.position;
                directionToPlayerOffLerp.y += 1;
                rootSniper.transform.right = directionToPlayerOffLerp;
            }
        }
        else
        {
            time += Time.deltaTime;
            rootSniper.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, patrolAngles[0]), Quaternion.Euler(0, 0, patrolAngles[1]), Mathf.Abs(Mathf.Sin(time * 0.5f)));
        }
        lineRenderer.SetPosition(0, lazerSocket.position);
        lineRenderer.SetPosition(1, directionToEndPoint);
    }

    IEnumerator TimerShotSniper()
    {
        isShotSniper = false;

        yield return new WaitForSeconds(1);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.25f);
        audioSourceWeapon.PlayOneShot(sniperShot);
        enemyWeapon.Shoot();

        lineRenderer.enabled = true;
        isShotSniper = true;
    }
    void DestroyEnemy()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(dead);

        GameObject Copy = Instantiate(deadCopy, transform.position, Quaternion.identity);
        Copy.GetComponent<DeadCopyEnemy>().spriteHead.sprite = spriteForDeadCopy[0].sprite;
        Copy.GetComponent<DeadCopyEnemy>().spriteBody.sprite = spriteForDeadCopy[1].sprite;
        TrajectoryRenderer.instance.RemoveBody(Rigidbody2D);
        Destroy(gameObject);
    }
}


