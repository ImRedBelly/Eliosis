using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        NONE,
        KNIFE,
        PISTOL,
        SHOTGUN,
        ASSAULT,
        SNIPER,
        MACHINEGUN
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

        public bool isInInventory;
        public Animator animator;
    }

    public string bulletMask;

    public WeaponSlot[] weapons;

    public SpriteRenderer hand1;
    public SpriteRenderer hand2;
    public SpriteRenderer handfist1;
    public SpriteRenderer handfist2;

    public WeaponType currentWeapon;
    private int numberOfWeapons;
    ReversLook reversLook;

    private float nextFire;

    private float hitDistance = 1f;
    private float hitAngle = 45f;

    private LayerMask whatIsEnemy;



    Animator currentWeaponAnimator;
    CharacterController2D controller;
    PlayerMovement player;



    [Space]
    [Header("Laser Aim")]
    LineRenderer lr;
    float rayLaserDistance = 5;





    [Space]
    [Header("Melee Attack")]
    public GameObject cam;
    public Transform attackCheck;
    public float damageValue = 1;

    public float MeleeDamage
    {
        get
        {
            return damageValue;
        }
        set
        {
            damageValue = value;
        }
    }

    private void Awake()
    {
        reversLook = GetComponent<ReversLook>();
        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerMovement>();
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        // todo создавать enum на старте
        numberOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;

        SetWeapon(WeaponType.NONE);

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ChangeWeapon();

            foreach (var item in PlayerMovement.instance.inventory.slots)
            {
                item.ApplyItemFromKey();
            }

        }

        ChangeDirection();

        CheckFire();
    }



    void ChangeDirection()
    {
        if (currentWeapon == WeaponType.NONE || currentWeapon == WeaponType.KNIFE)
        {
            return;
        }

        Transform placeWeapon = weapons[(int)currentWeapon].placeWeapon;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - placeWeapon.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        direction.z = 0;

        float angle = Vector2.SignedAngle(Vector2.right, direction);


        if (controller.isWallSliding && transform.localScale.x < 0)
        {

            if (angle < 135 && angle > 0)
            {
                placeWeapon.rotation =
                Quaternion.Euler(0, 0, -45);
                return;
            }

            if (angle > -135 && angle < 0)
            {
                placeWeapon.rotation =
                Quaternion.Euler(0, 0, 45);
                return;
            }
            placeWeapon.right = -direction;
            return;
        }


        if (controller.isWallSliding && transform.localScale.x > 0)
        {
            if (angle > 45)
            {
                placeWeapon.rotation =
                Quaternion.Euler(0, 0, 45);
                return;
            }

            if (angle < -45)
            {
                placeWeapon.rotation =
                Quaternion.Euler(0, 0, -45);
                return;
            }
            placeWeapon.right = direction;
            return;
        }






        if (!controller.isWallSliding && controller.m_IsWall && transform.localScale.x < 0)
        {
            placeWeapon.right = -direction;
            return;
        }

        if (!controller.isWallSliding && controller.m_IsWall && transform.localScale.x > 0)
        {
            placeWeapon.right = direction;
            return;
        }






        if (!controller.isWallSliding && transform.localScale.x < 0)
        {
            placeWeapon.right = -direction;
            return;
        }

        if (!controller.isWallSliding && transform.localScale.x > 0)
        {
            placeWeapon.right = direction;
            return;
        }


    }

    private void ChangeWeapon()
    {

        for (int i = (int)currentWeapon + 1; i < weapons.Length; i++)
        {
            if (weapons[i].isInInventory)
            {
                currentWeapon = weapons[i].weaponType;
                SetWeapon(currentWeapon);
                return;
            }
        }
       SetWeapon(WeaponType.NONE);
    }

    public void SetWeapon(WeaponType currentWeapon)
    {
        this.currentWeapon = currentWeapon;
        ShowWeapon((int)currentWeapon);
        SetWeaponAnimator((int)currentWeapon);
    }

    private void SetWeaponAnimator(int currentWeapon)
    {
        currentWeaponAnimator = weapons[currentWeapon].animator;
        PlayerMovement.instance.animatorWeapon = currentWeaponAnimator;
        controller.animatorWeapon = currentWeaponAnimator;

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

    private void Shoot()
    {
        //todo sound

        //вылетают пули
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
            GameObject bullet = Instantiate(weapons[(int)currentWeapon].bulletPrefab,
                            weapons[(int)currentWeapon].placeFire.position,
                            weapons[(int)currentWeapon].placeFire.rotation);

            bullet.GetComponent<Bullet>().direction = weapons[(int)currentWeapon].placeFire.right * transform.localScale.x * 5;
            bullet.gameObject.layer = LayerMask.NameToLayer(bulletMask);
        }



        //вылетает гильза
        GameObject shell = Instantiate(weapons[(int)currentWeapon].shellPrefab,
                                        weapons[(int)currentWeapon].placeShell.position,
                                        weapons[(int)currentWeapon].placeShell.rotation);


        Rigidbody2D shellRb = shell.GetComponent<Rigidbody2D>();
        shellRb.AddForce(weapons[(int)currentWeapon].placeShell.up * Random.Range(8, 12), ForceMode2D.Impulse);
        shellRb.AddTorque(Random.Range(-250, 250), ForceMode2D.Force);
        Destroy(shell, 3f);

        //сверкает вспышка
        GameObject flash = Instantiate(weapons[(int)currentWeapon].flashPrefab,
                                       weapons[(int)currentWeapon].placeFire.position,
                                       weapons[(int)currentWeapon].placeFire.rotation);
        flash.transform.right = flash.transform.right * transform.localScale.x * 5;

        Destroy(flash, 1f);
    }

    private void CheckFire()
    {
        //---------- MACHINEGUN

        if (Input.GetButtonDown("Fire1") && currentWeapon == WeaponType.MACHINEGUN)
        {
            weapons[(int)currentWeapon].animator.SetBool("IsAttacking", true);

            if (nextFire <= 0)
            {
                Shoot();
            }
        }

        if (Input.GetButtonUp("Fire1") && currentWeapon == WeaponType.MACHINEGUN)
        {
            weapons[(int)currentWeapon].animator.SetBool("IsAttacking", false);
        }


        //---------- SNIPER

        if (Input.GetButton("Fire1") && currentWeapon == WeaponType.SNIPER)
        {
            lr.enabled = true;
            RaycastHit2D rayLaser = Physics2D.Raycast(weapons[(int)currentWeapon].placeFire.position,
                                                      weapons[(int)currentWeapon].placeFire.right * transform.localScale.x * 5);

            if (rayLaser)
            {
                lr.SetPosition(0, weapons[(int)currentWeapon].placeFire.position);
                lr.SetPosition(1, rayLaser.point);
            }
            else
            {
                lr.SetPosition(0, weapons[(int)currentWeapon].placeFire.position);

                var dir = weapons[(int)currentWeapon].placeFire.position -
                                                      weapons[(int)currentWeapon].placeFire.right * transform.localScale.x * -100;

                lr.SetPosition(1, dir);
            }
            return;
        }

        if (Input.GetButtonUp("Fire1") && currentWeapon == WeaponType.SNIPER)
        {
            lr.enabled = false;
            Shoot();
        }


        //---------- KNIFE

        if (Input.GetButtonUp("Fire1") && currentWeapon == WeaponType.KNIFE && nextFire <= 0)
        {
            nextFire = weapons[(int)currentWeapon].fireRate;
            weapons[(int)currentWeapon].animator.SetBool("IsAttacking", true);
        }




        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            nextFire = weapons[(int)currentWeapon].fireRate;
            weapons[(int)currentWeapon].animator.SetTrigger("IsAttacking");

            if (currentWeapon == WeaponType.NONE || currentWeapon == WeaponType.KNIFE)
            {
                DoDashDamage();
                return;
            }

            Shoot();
        }

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
    }

    public void DoDashDamage()
    {
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
        }
    }

    public void UpdateMaxDamage(float damagePoint)
    {
        MeleeDamage += damagePoint;
    }

}

