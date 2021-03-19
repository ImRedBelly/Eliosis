using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class EnemyWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        KNIFE,
        SHOTGUN,
        ASSAULT,
        SNIPER
    }
    [Serializable]
    public class WeaponSlot
    {
        public WeaponType weaponType;
        public float fireRate;

        public GameObject bulletPrefab;
        public GameObject shellPrefab;
        public GameObject flashPrefab;

        public GameObject powerUp;
        public float weigth;
        public bool isMelee;
        public Transform placeWeapon;
        public Transform placeFire;
        public Transform placeShell;
        public Animator animator;
    }
    private float nextFire;
    public string bulletMask;


    public WeaponSlot[] weapons;


    public WeaponType currentWeapon;
    private int numberOfWeapons;

    Animator currentWeaponAnimator;
    EnemyController enemyController;

    PlayerMovement player;
    void Awake()
    {
        currentWeaponAnimator = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
        player = PlayerMovement.instance;
    }

    void Start()
    {
        numberOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;

        currentWeapon = WeaponType.SHOTGUN;

        SetWeaponAnimator((int)currentWeapon);
        ShowWeapon((int)currentWeapon);
    }



    void SetWeaponAnimator(int currentWeapon)
    {
        currentWeaponAnimator = weapons[currentWeapon].animator;
        enemyController.animatorWeapon = currentWeaponAnimator;
    }
    private void ShowWeapon(int currentWeapon)
    {
        HideAllWeapon();
        weapons[currentWeapon].placeWeapon.gameObject.SetActive(true);
    }
    private void HideAllWeapon()
    {
        for (int i = 0; i < numberOfWeapons; i++)
        {
            weapons[i].placeWeapon.gameObject.SetActive(false);
        }
    }


    public void CheckFire()
    {
        if (currentWeapon == WeaponType.KNIFE && nextFire <= 0)
        {
            nextFire = weapons[(int)currentWeapon].fireRate;
            enemyController.MeleeAttack();
            weapons[(int)currentWeapon].animator.SetTrigger("IsAttacking");
        }


        if (nextFire <= 0)
        {
            nextFire = weapons[(int)currentWeapon].fireRate;
            weapons[(int)currentWeapon].animator.SetTrigger("IsAttacking");

            if (currentWeapon == WeaponType.KNIFE)
                return;

            Shoot();
        }

        if (nextFire > 0)
            nextFire -= Time.deltaTime;

    }
    public void Shoot()
    {

        int numberOfBullets;
        if (currentWeapon == WeaponType.SHOTGUN)  // количество дроби
        {
            numberOfBullets = 5;
        }
        else
        {
            numberOfBullets = 1;
        }

        for (int i = 0; i < numberOfBullets; i++)
        {
            print(i);
            GameObject bullet = Instantiate(weapons[(int)currentWeapon].bulletPrefab,
                                        weapons[(int)currentWeapon].placeFire.position,
                                        weapons[(int)currentWeapon].placeFire.rotation);

            bullet.GetComponent<Bullet>().direction = weapons[(int)currentWeapon].placeFire.right * transform.localScale.x * 4;
            bullet.gameObject.layer = LayerMask.NameToLayer(bulletMask);
        }
        GameObject shell = Instantiate(weapons[(int)currentWeapon].shellPrefab,
                                        weapons[(int)currentWeapon].placeShell.position,
                                        weapons[(int)currentWeapon].placeShell.rotation);
        Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();
        shellRb.AddForce(weapons[(int)currentWeapon].placeShell.up * Random.Range(8, 12), ForceMode2D.Impulse);
        shellRb.AddTorque(Random.Range(-250, 250), ForceMode2D.Force);
        Destroy(shell, 3f);


        GameObject flash = Instantiate(weapons[(int)currentWeapon].flashPrefab,
                                      weapons[(int)currentWeapon].placeFire.position,
                                      weapons[(int)currentWeapon].placeFire.rotation);
        flash.transform.right = flash.transform.right * transform.localScale.x * 5;

        Destroy(flash, 1f);
    }





    public void SetKnife()
    {
        currentWeapon = WeaponType.KNIFE;

        ShowWeapon((int)currentWeapon);
        SetWeaponAnimator((int)currentWeapon);
    }
    public void SetAssault()
    {
        currentWeapon = WeaponType.ASSAULT;

        ShowWeapon((int)currentWeapon);
        SetWeaponAnimator((int)currentWeapon);
    }
    public void SetShotgun()
    {
        currentWeapon = WeaponType.SHOTGUN;

        ShowWeapon((int)currentWeapon);
        SetWeaponAnimator((int)currentWeapon);
    }
    public void SetSniper()
    {
        currentWeapon = WeaponType.SNIPER;

        ShowWeapon((int)currentWeapon);
        SetWeaponAnimator((int)currentWeapon);
    }
}


