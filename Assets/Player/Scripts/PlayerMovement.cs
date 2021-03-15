using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    public CharacterController2D controller;
    public Animator animator;
    public Animator animatorWeapon;


    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;


    [Header("Bash")]
    [SerializeField] private float Raduis;
    [SerializeField] GameObject BashAbleObj;
    public bool NearToBashAbleObj;
    private bool IsChosingDir;
    private bool IsBashing;
    [SerializeField] private float BashPower;
    [SerializeField] private float BashTime;
    [SerializeField] private GameObject Arrow;
    Vector3 BashDir;
    private float BashTimeReset;


    [Header("Teleport")]
    public SpriteRenderer spriteRenderer;
    Material material;
    float fade = 1f;
    Coroutine start;
    Coroutine end;


    [Header("Shield")]
    public GameObject shield;


    [Header("Ledge")]
    public bool isLedge = false;


   


    private void Start()
    {
        instance = this;
        BashTimeReset = BashTime;
        material = spriteRenderer.material;
    }
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        animatorWeapon.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = true;
        }

        Bash();



        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ShieldActiv());
        }
    }
    IEnumerator ShieldActiv()
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(3);
        shield.SetActive(false);
    }
    public void OnFall()
    {
        animator.SetBool("IsJumping", true);
        animatorWeapon.SetBool("IsJumping", true);

    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animatorWeapon.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        if (IsBashing == false)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
            jump = false;
            dash = false;
        }

    }

    public void Ledge(Vector2 ledge)
    {
        StartCoroutine(JumpLedge(ledge));
    }
    IEnumerator JumpLedge(Vector2 ledge)
    {
        controller.m_Rigidbody2D.AddForce(ledge * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        controller.m_Rigidbody2D.AddForce(ledge * 1.2f, ForceMode2D.Impulse);

    }

    void Bash()
    {
        RaycastHit2D[] Rays = Physics2D.CircleCastAll(transform.position, Raduis, Vector3.forward);
        foreach (RaycastHit2D ray in Rays)
        {
            NearToBashAbleObj = false;

            if (ray.collider.tag == "BashPoint")
            {
                NearToBashAbleObj = true;
                BashAbleObj = ray.collider.transform.gameObject;
                break;
            }
        }
        if (NearToBashAbleObj)
        {
            BashAbleObj.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (Input.GetMouseButtonDown(1))
            {
                Time.timeScale = 0;
                BashAbleObj.transform.localScale = new Vector2(0.12f, 0.12f);
                Arrow.SetActive(true);
                Arrow.transform.position = BashAbleObj.transform.transform.position;
                IsChosingDir = true;
            }
            else if (IsChosingDir && Input.GetMouseButtonUp(1))
            {
                Time.timeScale = 1f;
                BashAbleObj.transform.localScale = new Vector2(0.1f, 0.1f);
                IsChosingDir = false;
                IsBashing = true;
                transform.position = BashAbleObj.transform.position;
                BashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                BashDir.z = 0;
                BashDir = BashDir.normalized;

                print(BashDir.magnitude);

                BashAbleObj.GetComponent<Rigidbody2D>().AddForce(-BashDir * 1, ForceMode2D.Impulse);
                Arrow.SetActive(false);

            }
        }
        else if (BashAbleObj != null)
        {
            BashAbleObj.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (IsBashing)
        {
            if (BashTime > 0)
            {
                BashTime -= Time.deltaTime;
                //controller.m_Rigidbody2D.velocity = Vector2.zero;
                controller.m_Rigidbody2D.velocity = BashDir * BashPower * Time.deltaTime;
            }
            else
            {
                IsBashing = false;
                BashTime = BashTimeReset;
                controller.m_Rigidbody2D.velocity = new Vector2(controller.m_Rigidbody2D.velocity.x, 0);
            }
        }
    }


    public void TeleportMinus()
    {
        start = StartCoroutine(Minus());
    }
    IEnumerator Minus()
    {
        while (fade >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            fade -= 0.1f;
            if (fade < 0)
            {
                StopCoroutine(start);
                fade = 0;
            }
            material.SetFloat("_Fade", fade);
        }
    }
    public void TeleportPlus()
    {
        end = StartCoroutine(Plus());
    }
    IEnumerator Plus()
    {
        while (fade <= 1)
        {
            yield return new WaitForSeconds(0.1f);
            fade += 0.1f;
            if (fade > 1)
            {
                StopCoroutine(end);
                fade = 1;
            }
            material.SetFloat("_Fade", fade);
        }
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, Raduis);
    //}

}
