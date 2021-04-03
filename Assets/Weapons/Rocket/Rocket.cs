using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    RocketMovement rocketMovement;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        rocketMovement = PlayerMovement.instance.GetComponent<RocketMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print(collision.gameObject);
            rocketMovement.DamageOnTrigger();
            audioSource.Play();
        }

    }
}
