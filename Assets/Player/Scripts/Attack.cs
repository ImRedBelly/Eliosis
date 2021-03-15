using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public Slider damageSlider;

    public float damageValue = 1;
    float sliderMaxValue = 50;

    public Transform attackCheck;
    public Animator animatorWeapon;
    public bool canAttack = true;
    public bool isTimeToCheck = false;

    public GameObject cam;

    public float Damage
    {
        get
        {
            return damageValue;
        }
        set
        {
            damageValue = value;
            damageSlider.value = damageValue;
        }
    }
    private void Start()
    {
        damageSlider.maxValue = sliderMaxValue;
        damageSlider.value = damageValue;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            animatorWeapon.SetBool("IsAttacking", true);
            DoDashDamage();
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

    public void DoDashDamage()
    {
        print("attack");
        damageValue = Mathf.Abs(damageValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy" || collidersEnemies[i].gameObject.tag == "DeathCopy")
            {
                if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                {
                    damageValue = -damageValue;
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", damageValue);
                cam.GetComponent<CameraFollow>().ShakeCamera();
            } 
            
            




            if (collidersEnemies[i].gameObject.tag == "Torch")
            {
                collidersEnemies[i].GetComponent<Torch>().Fire();
            }
            //if (collidersEnemies[i].gameObject.tag == "DestoyCube")
            //{
            //    collidersEnemies[i].GetComponent<DestroyCube>().Destroy();
            //}
        }
    }


    public void UpdateMaxDamage(float damagePoint)
    {
        Damage += damagePoint;
    }
}
