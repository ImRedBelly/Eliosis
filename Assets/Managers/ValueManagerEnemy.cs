using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueManagerEnemy : MonoBehaviour
{
    public static ValueManagerEnemy instance;
    public BulletValue bulletValue;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


    [Serializable]
    public class BulletValue
    {
        public float speed = 20;
        public float damageBullet = 1;
        public float Damage
        {
            get
            {
                return damageBullet;
            }
            set
            {
                damageBullet = value;
            }
        }

        public void UpdateMaxDamage(float damagePoint)
        {
            Damage += damagePoint;
        }
    }
}
