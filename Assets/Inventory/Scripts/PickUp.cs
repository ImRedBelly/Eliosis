using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public Weapon.WeaponType weaponType;

    [Space]

    [SerializeField] public GameObject itemIconForMainSlot; // для главного слота c руками
    [SerializeField] public GameObject itemIconForSlot;
    [SerializeField] public bool isOneOff; // например, аптечка. Оружие -нет.
    [SerializeField] public string itemName;
    [SerializeField] public int itemWeigth;


    //[SerializeField] private GameObject Glow;

    private bool isTriggered;

    private Inventory inventory;

    private Transform appliedInventoryPlace;


    private void Start()
    {
        //Glow.SetActive(false);
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L) && isTriggered)
        {
            PlayerMovement.instance.inventory.PutInInventory(this);

            Destroy(gameObject);
        }
    }

    public void ApplyPickUp() // у каждого свой метод
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isTriggered = true;
            //PossibilityToTake(isTriggered);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = false;
           // PossibilityToTake(isTriggered);
        }
    }

    //private void PossibilityToTake(bool enable)
    //{
    //    Glow.SetActive(enable);
    //    print("могу взять" + enable);
    //}

}
