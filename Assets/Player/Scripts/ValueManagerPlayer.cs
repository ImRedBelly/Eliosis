using System;
using UnityEngine;
using UnityEngine.UI;

public class ValueManagerPlayer : MonoBehaviour
{
    public static ValueManagerPlayer instance;
    public BulletValue bulletValue;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        bulletValue.StartSlider();
    }

    [Serializable]
    public class BulletValue
    {
        public Slider damageSlider;
        float sliderMaxValue = 30;

        public float speed = 20;
        public float damageBullet = 2;
        public float Damage
        {
            get
            {
                return damageBullet;
            }
            set
            {
                damageBullet = value;
                damageSlider.value = damageBullet;
            }
        }

        public void StartSlider()
        {
            //print("Start");
            damageSlider.maxValue = sliderMaxValue;
            damageSlider.value = damageBullet;
        }
        public void UpdateMaxDamage(float damagePoint)
        {
            Damage += damagePoint;
        }
    }


}
