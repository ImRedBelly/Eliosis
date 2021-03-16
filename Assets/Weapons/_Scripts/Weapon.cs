using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public GameObject powerUp;
		public float weigth;
		public bool isMelee;
		public Transform placeWeapon;
		public Transform placeFire;
		public Animator animator;
	}

	public WeaponSlot[] weapons;

	public SpriteRenderer hand1;
	public SpriteRenderer hand2;
	public SpriteRenderer handfist1;
	public SpriteRenderer handfist2;

	private WeaponType currentWeapon;
	private int numberOfWeapons;
	ReversLook reversLook;

	private float nextFire;

	private float hitDistance = 1f;
	private float hitAngle = 45f;

	private LayerMask whatIsEnemy;

	private void Awake()
	{
		reversLook = GetComponent<ReversLook>();
	}

	private void Start()
	{
		// todo создавать enum на старте
		numberOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;

		currentWeapon = WeaponType.NONE;

		ShowWeapon((int)currentWeapon);
	}



	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeWeapon();
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

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePosition - weapons[(int)currentWeapon].placeWeapon.position;
		direction.z = 0;

		if (reversLook.isReversLook)
		{
			weapons[(int)currentWeapon].placeWeapon.right = direction;
		}
		else
		{
			weapons[(int)currentWeapon].placeWeapon.right = -direction;
		}
	}

	private void ChangeWeapon()
	{
		currentWeapon++;

		// todo ротировть только те что находятся в инвентаре
		if ((int)currentWeapon == numberOfWeapons)
		{
			currentWeapon = 0;
		}

		ShowWeapon((int)currentWeapon);

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

	public void Shoot()
	{
		//todo sound

		GameObject bullet = Instantiate(weapons[(int)currentWeapon].bulletPrefab,
			weapons[(int)currentWeapon].placeFire.position, weapons[(int)currentWeapon].placeFire.rotation);
	}

	private void CheckFire()
	{
		if (currentWeapon == WeaponType.MACHINEGUN)
		{
			if (Input.GetButtonDown("Fire1") ) 
			{
				weapons[(int)currentWeapon].animator.SetBool("IsAttacking", true);

				if (nextFire <= 0)
				{
					Shoot();
				}

			}

			if (Input.GetButtonUp("Fire1"))
			{
				weapons[(int)currentWeapon].animator.SetBool("IsAttacking", false);
			}
		}
		else if (Input.GetButton("Fire1") && nextFire <= 0)
		{
			nextFire = weapons[(int)currentWeapon].fireRate;
			weapons[(int)currentWeapon].animator.SetTrigger("IsAttacking");



			if (currentWeapon == WeaponType.NONE || currentWeapon == WeaponType.KNIFE)
			{
				return;
			}

			Shoot();
		}

		if (nextFire > 0)
		{
			nextFire -= Time.deltaTime;
		}
	}

}

