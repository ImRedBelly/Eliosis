using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
   public RocketMovement rocketMovement;

    private void Start()
    {
        rocketMovement = FindObjectOfType<RocketMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rocketMovement.DamageOnTrigger();
        }
    }
}
