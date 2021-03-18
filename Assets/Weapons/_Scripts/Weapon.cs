﻿using System;
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
		public GameObject shellPrefab;
		public GameObject powerUp;
		public float weigth;
		public bool isMelee;
		public Transform placeWeapon;
		public Transform placeFire;
		public Transform placeShell;
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

	Animator currentWeaponAnimator;
	CharacterController2D controller;
	PlayerMovement player;

	private void Awake()
	{
		reversLook = GetComponent<ReversLook>();
		controller = GetComponent<CharacterController2D>();
		player = GetComponent<PlayerMovement>();

	}

	private void Start()
	{
		// todo создавать enum на старте
		numberOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;

		currentWeapon = WeaponType.NONE;
		SetWeaponAnimator((int)currentWeapon);
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

		Transform placeWeapon = weapons[(int)currentWeapon].placeWeapon;

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 direction = mousePosition - placeWeapon.position;

		Quaternion targetRotation = Quaternion.LookRotation(direction);

		direction.z = 0;

		float angle = Vector2.SignedAngle(Vector2.right, direction);
		print(angle);


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
		currentWeapon++;

		// todo ротировть только те что находятся в инвентаре
		if ((int)currentWeapon == numberOfWeapons)
		{
			currentWeapon = 0;
		}

		ShowWeapon((int)currentWeapon);
		SetWeaponAnimator((int)currentWeapon);



	}

	void SetWeaponAnimator(int currentWeapon)
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

	public void Shoot()
	{
		//todo sound

		GameObject bullet = Instantiate(weapons[(int)currentWeapon].bulletPrefab,
										weapons[(int)currentWeapon].placeFire.position,
										weapons[(int)currentWeapon].placeFire.rotation);

		GameObject shell = Instantiate(weapons[(int)currentWeapon].shellPrefab,
										weapons[(int)currentWeapon].placeFire.position,
										weapons[(int)currentWeapon].placeFire.rotation);


	}

	private void CheckFire()
	{




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

