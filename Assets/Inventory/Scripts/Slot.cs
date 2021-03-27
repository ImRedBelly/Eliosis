using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool isFull;

    public GameObject placeForIcon;
    public GameObject gameObjectPickUp;

    public Text textName;
    public Text textWeigth;

    public int numberOfItems;
    public Text textNumberOfItems;

    public PickUp pickUp;

    MainSlot mainSlot;
    Inventory inventory;
    public Weapon weapon;

    Weapon Weapon
    {
        get
        {
            if (weapon == null)
            {
                weapon = PlayerMovement.instance.weapon;
            }
            return weapon;
        }


    }

    Inventory Inventory
    {
        get
        {
            if (inventory == null)
            {
                inventory = PlayerMovement.instance.inventory;
            }
            return inventory;
        }
    }

    MainSlot MainSlot
    {
        get
        {
            if (mainSlot == null)
            {
                mainSlot = PlayerMovement.instance.inventory.mainSlot;
            }
            return mainSlot;
        }
    }


    private void Start()
    {
        gameObject.SetActive(isFull);
        textNumberOfItems.gameObject.SetActive(numberOfItems > 1);
        textNumberOfItems.text = $"х {numberOfItems}";
    }


    public void AddItem(PickUp pickUp)
    {
        Weapon.weapons[(int)pickUp.weaponType].isInInventory = true;


        gameObjectPickUp = Instantiate(pickUp.gameObject, Inventory.placeItem);
        gameObjectPickUp.SetActive(false);

        this.pickUp = gameObjectPickUp.GetComponent<PickUp>();

        isFull = true;

        AddItem();

        Instantiate(pickUp.itemIconForSlot, placeForIcon.transform);
        textName.text = pickUp.itemName;
        textWeigth.text = pickUp.itemWeigth.ToString();

    }

    public void AddItem()
    {
        AddNumberOfItems();
    }

    void AddNumberOfItems()
    {
        ++numberOfItems;

        inventory.AddToTotalWeight(pickUp.itemWeigth);

        textNumberOfItems.gameObject.SetActive(numberOfItems > 1);

        textNumberOfItems.text = $"х {numberOfItems}";
    }

    void SubNumberOfItems()
    {
        --numberOfItems;

        textNumberOfItems.gameObject.SetActive(numberOfItems > 1);

        textNumberOfItems.text = $"х {numberOfItems}";
    }


    public void RemoveItem()
    {
        if (!isFull)
        {
            return;
        }

        SubNumberOfItems();
        inventory.SubFromTotalWeight(pickUp.itemWeigth);

        ItemThrowOnScene();

        if (numberOfItems == 0)
        {
            Weapon.weapons[(int)pickUp.weaponType].isInInventory = false;
            ClearSlot();
        }

    }

    private void ClearSlot()
    {
        isFull = false;


        Destroy(placeForIcon.transform.GetChild(0).gameObject);

        textName.text = "";
        textWeigth.text = "";

        gameObject.SetActive(false);

        pickUp = null;
    }



    public void ApplyItem()
    {
        if (!isFull)
        {
            return;
        }

        SubNumberOfItems();

        Weapon.SetWeapon(pickUp.weaponType);

        if (pickUp.isOneOff)
        {
            inventory.SubFromTotalWeight(pickUp.itemWeigth);

            if (numberOfItems > 0)
            {
                return;
            }

            ClearSlot();
            return;
        }

        ChangeItemsSlotAndInventory();

        if (numberOfItems > 0)
        {
            return;
        }

        ClearSlot();
    }

    public void ApplyItemFromKey()
    {

        if (Weapon.currentWeapon == Weapon.WeaponType.NONE)
        {
            if (MainSlot.isFull)
            {
                MainSlot.PutCurrentItemToInventory();
                MainSlot.ClearSlot();
                MainSlot.ShowHand();
            }


        }

        if (!isFull)
        {
            return;
        }
;



        if (Weapon.currentWeapon == pickUp.weaponType)
        {

            SubNumberOfItems();

            ChangeItemsSlotAndInventory();

            if (numberOfItems > 0)
            {
                return;
            }

            ClearSlot();

        }
    }

    public void ChangeItemsSlotAndInventory()
    {
        if (!MainSlot.isFull)
        {
            MainSlot.AddItem(pickUp);
            return;
        }

        MainSlot.PutCurrentItemToInventory();
        MainSlot.ClearSlot();
        MainSlot.AddItem(pickUp);
    }

    private void ItemThrowOnScene()
    {
        GameObject clone = Instantiate(pickUp.gameObject, inventory.placeItem.position
        + new Vector3(1, 0, 0), Quaternion.identity);
        clone.SetActive(true);
    }

}


