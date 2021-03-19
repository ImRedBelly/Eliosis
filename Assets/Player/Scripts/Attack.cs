using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{


    public Animator animatorWeapon;
    public bool canAttack = true;
    public bool isTimeToCheck = false;


   
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            //animatorWeapon.SetBool("IsAttacking", true);
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canAttack = true;
    }

 
    }


    

