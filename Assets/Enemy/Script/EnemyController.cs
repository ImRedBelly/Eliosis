using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public bool isSniper;
    public GameObject rootSniper;


    float timeLaser = 4;
    [Header("Components")]
    public Animator animator;
    public Rigidbody2D Rigidbody2D;
    public Animator animatorWeapon;
    public EnemyWeapon enemyWeapon;

    public LineRenderer lineRenderer;

    [Header("Options")]
    public float life = 5;
    public float speed = 5f;

    private bool facingRight = true;

    [Header("Attack")]
    public Transform attackCheck;
    public Transform placeFire;

    public GameObject enemy;
    //public GameObject bullet;

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

    float time = 0;
    private void Start()
    {
        activState = BossState.IDLE;
    }

    void FixedUpdate()
    {

        if (life <= 0)
            StartCoroutine(DestroyEnemy());

        else if (enemy != null)
        {
            distToPlayer = enemy.transform.position.x - transform.position.x;

            if (isSniper)
            {
                Idle();
                time += Time.deltaTime;
                rootSniper.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 30), Quaternion.Euler(0, 0, -30), Mathf.Abs(Mathf.Sin(time * 0.5f)));

                //if (Mathf.Abs(distToPlayer) < snipeDist)
                // ShotSniper();


                // СДЕЛАТЬ ТАЙММЕР ДЛЯ ЛАЗЕРА, обычно включен, а при выстреле выключается чЧЕРЕЗ КОРОТИНУ
                // ТУТ НАДО СДЕЛАТЬ ЛАЗЕР ВРАГУ
                // ТУТ НАДО СДЕЛАТЬ ЛАЗЕР ВРАГУ
                // ТУТ НАДО СДЕЛАТЬ ЛАЗЕР ВРАГУ
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


    //public void ShootAttack(Vector2 position)
    //{
    //    GameObject throwable = Instantiate(bullet, attackCheck.position, Quaternion.identity);
    //    throwable.GetComponent<ElectricBullet>().direction = position;
    //}

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

        if (timeLaser > 0)
        {
            timeLaser -= Time.deltaTime;
            lineRenderer.enabled = true;

            RaycastHit2D rayLaser = Physics2D.Raycast(placeFire.position, placeFire.right * transform.localScale.x * 4);

            if (rayLaser)
            {
                lineRenderer.SetPosition(0, placeFire.position);
                lineRenderer.SetPosition(1, rayLaser.point);
            }
            else
            {
                lineRenderer.SetPosition(0, placeFire.position);
                var dir = placeFire.position - placeFire.right * transform.localScale.x * -100;

                lineRenderer.SetPosition(1, dir);
            }
        }
        else
        {
            lineRenderer.enabled = false;
            enemyWeapon.Shoot();
            timeLaser = 4;
            // StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);

    }
    IEnumerator DestroyEnemy()
    {
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.size = new Vector2(1f, 0.25f);
        capsule.offset = new Vector2(0f, -0.8f);
        capsule.direction = CapsuleDirection2D.Horizontal;
        transform.GetComponent<Animator>().SetBool("IsDead", true);
        yield return new WaitForSeconds(0.25f);
        Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}


