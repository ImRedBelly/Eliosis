using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum WeaponType { 
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
	}

	public WeaponSlot[] weapons;

	public SpriteRenderer hand1;
	public SpriteRenderer hand2;
	public SpriteRenderer handfist1;
	public SpriteRenderer handfist2;

	private WeaponType currentWeapon;
	private int numberOfWeapons;



	private void Start()
	{
		// todo создавать enum на старте
		numberOfWeapons = Enum.GetNames(typeof(WeaponType)).Length;
		currentWeapon = WeaponType.NONE;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeWeapon();
		}
	}

	private void ChangeWeapon()
	{
		currentWeapon++;

		// todo ротировть только те что находятся в инвентаре
		if ( (int) currentWeapon == numberOfWeapons ) 
		{
			currentWeapon = 0;
		}

		ShowWeapon((int) currentWeapon);
		

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
}

