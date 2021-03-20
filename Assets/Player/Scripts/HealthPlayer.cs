using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    public Slider healthSlider;

    [Header("Options")]
    public bool imLife = true;
    public bool invincible = false;

    public float health = 15;

    float minHealth = 0;
    float maxHealth = 15;

    float sliderMaxValue = 50; //максимально допустимое значение
    public float Life
    {
        get
        {
            healthSlider.value = maxHealth;
            return health;
        }
        set
        {
            if (value <= minHealth)
            {
                health = minHealth;
                StartCoroutine(WaitToDead());
            }

            else if (value >= maxHealth)
            {
                health = maxHealth;
            }

            else
            {
                health = value;
                StartCoroutine(Stun(0.25f));
                StartCoroutine(MakeInvincible(1f));
            }
        }
    }

    Rigidbody2D rb;
    Animator animator;
    CharacterController2D controller2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller2D = GetComponent<CharacterController2D>();


        healthSlider.maxValue = sliderMaxValue;
        healthSlider.value = maxHealth;
    }


    public void ApplyDamage(float damage, Vector3 position)
    {
        if (!invincible)
        {
            Life -= damage;
            Vector2 damageDir = Vector3.Normalize(transform.position - position) * 40f;
            rb.velocity = Vector2.zero;
            rb.AddForce(damageDir * 10);
        }

    }

    public void Healing(float healthPoint)
    {
        Life += healthPoint;
    }

    public void UpdateMaxHealth(float healthPoint)
    {
        maxHealth += healthPoint;
        healthSlider.value = maxHealth;
    }





    IEnumerator MakeInvincible(float time)
    {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    IEnumerator Stun(float time)
    {
        controller2D.canMove = false;
        yield return new WaitForSeconds(time);
        controller2D.canMove = true;
    }

    IEnumerator WaitToDead()
    {
        animator.SetBool("IsDead", true);
        controller2D.canMove = false;
        invincible = true;
        GetComponent<Attack>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        rb.velocity = new Vector2(0, rb.velocity.y);

        GameManager.instance.SavePosition();

        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(0);
    }
}
