using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject powerUp;
        public float weigth;
        public bool isMelee;
        public Transform placeWeapon;
        public Transform placeFire;
        public Transform placeShell;
        public Animator animator;
    }
    private float nextFire;


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
        GameObject bullet = Instantiate(weapons[(int)currentWeapon].bulletPrefab,
                                        weapons[(int)currentWeapon].placeFire.position,
                                        weapons[(int)currentWeapon].placeFire.rotation);

        GameObject shell = Instantiate(weapons[(int)currentWeapon].shellPrefab,
                                        weapons[(int)currentWeapon].placeFire.position,
                                        weapons[(int)currentWeapon].placeFire.rotation);

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


