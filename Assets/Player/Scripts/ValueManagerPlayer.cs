using System;
using UnityEngine;
using UnityEngine.UI;

public class ValueManagerPlayer : MonoBehaviour
{
    public static ValueManagerPlayer instance;
    public BulletValue bulletValue;
    public WeightValue weightValue;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        bulletValue.StartSlider();
        weightValue.StartSlider();
    }

    [Serializable]
    public class BulletValue
    {
        public Slider damageSlider;
        float sliderMaxValue = 15;

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
            damageSlider.maxValue = sliderMaxValue;
            damageSlider.value = damageBullet;
        }
        public void UpdateMaxDamage(float damagePoint)
        {
            Damage += damagePoint;
        }
    }
    [Serializable]
    public class WeightValue
    {
        public Slider weightSlider;
        float weightMaxValue = 15;

        public float weightPlayer = 2;
        public float Damage
        {
            get
            {
                return weightPlayer;
            }
            set
            {
                weightPlayer = value;
                weightSlider.value = weightPlayer;
            }
        }

        public void StartSlider()
        {
            weightSlider.maxValue = weightMaxValue;
            weightSlider.value = weightPlayer;
        }
        public void UpdateMaxWeight(float weightPoint)
        {
            Damage += weightPoint;
        }
    }
}
