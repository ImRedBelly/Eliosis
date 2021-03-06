using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeadCopy : MonoBehaviour
{
    public int money = 0;
    bool returnedTheMoney = false;

    [Header("Components")]
    public Animator animator;
    public Animator weaponAnimator;

    [Header("Options")]
    public float life = 10;
    public float speed = 5f;

    private bool facingRight = true;

    [Header("Attack")]
    public GameObject enemy;

    private float distToPlayer;
    private float distToPlayerY;

    public Transform attackCheck;
    public float dmgValue = 4;
    public float meleeDist = 1.5f;
    private bool canAttack = true;

    bool isEnemy;
    public LayerMask enemyMask;
    [Header("Fly Fire")]
    public GameObject[] fire;

    public GameObject deadEffect;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        money = GameManager.instance.countMoney;
        returnedTheMoney = false;
    }
    void FixedUpdate()
    {
        if (life <= 0)
        {
            StartCoroutine(DestroyEnemy());
        }

        else if (enemy != null)
        {
            distToPlayer = enemy.transform.position.x - transform.position.x;
            distToPlayerY = enemy.transform.position.y - transform.position.y;

            isEnemy = Physics2D.OverlapCircle(transform.position, 10, enemyMask);

            for (int i = 0; i < fire.Length; i++)
            {
                if (transform.localScale.x < 0)
                    fire[i].transform.rotation = Quaternion.Euler(0, 0, -90);

                else
                    fire[i].transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            if (isEnemy)
            {
                Run();

                if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                    Flip();

                if (Mathf.Abs(distToPlayer) > 0.25f && Mathf.Abs(distToPlayer) < meleeDist && Mathf.Abs(distToPlayerY) < 2f) // Melee Damage
                {
                    if (canAttack)
                    {
                        weaponAnimator.SetTrigger("IsAttacking");
                        MeleeAttack();
                    }

                }
            }

        }
        else
        {
            enemy = GameObject.Find("Player _Yura");
        }
    }


    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage)
    {
        float direction = damage / Mathf.Abs(damage);
        damage = Mathf.Abs(damage);
        life -= damage;
    }

    public void MeleeAttack()
    {


        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy" && collidersEnemies[i].gameObject != gameObject)
            {
                if (transform.localScale.x < 1)
                {
                    dmgValue = -dmgValue;
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
            }
            else if (collidersEnemies[i].gameObject.tag == "Player")
            {
                collidersEnemies[i].gameObject.GetComponent<HealthPlayer>().ApplyDamage(2f, transform.position);
            }
        }
        StartCoroutine(WaitToAttack(0.5f));
    }



    public void Run()
    {
        transform.position = Vector2.Lerp(transform.position, enemy.transform.position, 0.01f);
    }


    IEnumerator WaitToAttack(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }


    IEnumerator DestroyEnemy()
    {
        if (!returnedTheMoney)
        {
            PlayerMovement.instance.GetComponent<Purse>().TakeMoney(money);
            returnedTheMoney = true;
        }

        TrajectoryRenderer.instance.RemoveBody(rb);
        yield return new WaitForSeconds(0.1f);
        Instantiate(deadEffect, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
