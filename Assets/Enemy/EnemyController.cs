using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D Rigidbody2D;
    public Animator animator;
    public Animator animatorHand;


    [Header("Options")]
    public float life = 5;
    public float speed = 5f;

    private bool facingRight = true;

    [Header("Melee Attack")]
    float timerIsAttack = 4;
    float timerMeleeAttack = 4;

    [Header("Attack")]
    public Transform attackCheck;

    public GameObject enemy;
    public GameObject bullet;

    private float distToPlayer;

    public float dmgValue = 4;
    public float meleeDist = 1.5f;

    private bool canAttack = true;


    public BossState activState;
    public enum BossState
    {
        IDLE,
        MELEEATTACK,
        SHOOT
    }
    private void Start()
    {
        activState = BossState.IDLE;
    }

    void FixedUpdate()
    {
        // print(Mathf.Abs(distToPlayer));

        if (life <= 0)
            StartCoroutine(DestroyEnemy());

        else if (enemy != null)
        {
            distToPlayer = enemy.transform.position.x - transform.position.x;

            if (Mathf.Abs(distToPlayer) < meleeDist && timerMeleeAttack <= 0)
            {
                activState = BossState.MELEEATTACK;
                timerMeleeAttack = 3;
            }
            else
            {
                timerMeleeAttack -= Time.deltaTime;
            }

            switch (activState)
            {
                case BossState.IDLE:
                    Idle();
                    if (Mathf.Abs(distToPlayer) < 10)
                    {

                    }


                    if (timerIsAttack > 0)
                        timerIsAttack -= Time.deltaTime;
                    else
                    {
                        //int randomAct = Random.Range(0, 2);
                        //if (randomAct == 0)
                        //{
                        //    print("0");

                        //    activState = BossState.MELEEEASYATTACK;

                        //    animator.SetTrigger("MeleeEasyAttack");
                        //    animatorHand.SetTrigger("MeleeEasyAttack");
                        //}
                        //else if (randomAct == 1)
                        //{
                        //    print("1");

                        //    activState = BossState.MELEEHARDATTACK;

                        //    animator.SetTrigger("MeleeHardAttack");
                        //    animatorHand.SetTrigger("MeleeHardAttack");
                        //}
                        //else if (randomAct == 2)
                        //{
                        //    activState = BossState.MOVE;
                        //}
                    }
                    break;


                case BossState.SHOOT:

                    animator.SetBool("IsWaiting", false);
                    animatorHand.SetBool("IsWaiting", false);

                    Run(distToPlayer);

                    if (Mathf.Abs(distToPlayer) > 0.25f && Mathf.Abs(distToPlayer) < meleeDist)
                        activState = BossState.MELEEATTACK;
                    break;



                case BossState.MELEEATTACK:

                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Rigidbody2D.velocity.y);
                    if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))
                        Flip();


                    if (canAttack)
                    {
                        //MeleeAttack()        вызывается с ивента
                        animator.SetTrigger("MeleeAttack");
                        animatorHand.SetTrigger("MeleeAttack");
                        StartCoroutine(WaitToAttack(0.5f));
                    }


                    break;
            }
        }
        else
        {
            enemy = GameObject.Find("DrawCharacter");
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
        damage = Mathf.Abs(damage);
        animator.SetBool("Hit", true);
        life -= damage;
    }
    public void MeleeAttack()
    {
        print("Melee Atatack");
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
        animatorHand.SetBool("IsWaiting", false);
        Rigidbody2D.velocity = new Vector2(position / Mathf.Abs(position) * speed, Rigidbody2D.velocity.y);
    }

    public void Idle()
    {
        Rigidbody2D.velocity = new Vector2(0f, Rigidbody2D.velocity.y);
        animator.SetBool("IsWaiting", true);
        animatorHand.SetBool("IsWaiting", true);
    }



    IEnumerator WaitToAttack(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
        activState = BossState.IDLE;
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (enemy != null)
        {
            Gizmos.DrawLine(transform.position, enemy.transform.position);
        }

    }
}


