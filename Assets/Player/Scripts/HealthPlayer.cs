using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour
{
    public Slider healthSlider;

    public float health = 15;

    float minHealth = 0;
    float maxHealth = 15;
    float sliderMaxValue = 30; //максимально допустимое значение

    bool invincible = false;

    public GameObject[] playerComponent;
    public GameObject effectDead;
    GameObject emptyEffectDead;
    public float Life
    {
        get
        {
            healthSlider.value = health;
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
    CharacterController2D controller2D;
    public GameObject deadCopy;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (emptyEffectDead == null)
        {
            emptyEffectDead = Instantiate(effectDead, transform.position, Quaternion.identity);
            emptyEffectDead.transform.localScale = new Vector2(2, 2);
        }

        playerComponent[0].SetActive(false);
        playerComponent[1].SetActive(false);
        rb.velocity = new Vector2(0, rb.velocity.y);

        GameManager.instance.SavePosition();

        yield return new WaitForSeconds(1f);

        playerComponent[0].SetActive(true);
        playerComponent[1].SetActive(true);

        DeathController.deathController.CreadDeadCopy();
        DeathController.deathController.LoadCheckPoint(gameObject);
        health = maxHealth;
        healthSlider.value = maxHealth;
    }
}
