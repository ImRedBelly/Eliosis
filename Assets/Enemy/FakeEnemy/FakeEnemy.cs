using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeEnemy : MonoBehaviour
{
    public float health = 15;

    public void ApplyDamage(float damage)
    {
        print("Dead");
        damage = Mathf.Abs(damage);
        health -= damage;
    }
}
