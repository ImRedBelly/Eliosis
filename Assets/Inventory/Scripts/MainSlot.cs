using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSlot : MonoBehaviour
{
	public bool isFull;

	public Transform placeItemIconForMainSlot;

    public GameObject emptyHand;

    Inventory inventory;

    PickUp pickUp;

    Weapon weapon;
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

    public void AddItem(PickUp pickUp)
	{
        isFull = true;

        this.pickUp = pickUp;
        print("AddItem in Main: " + pickUp.weaponType);

        emptyHand.SetActive(false);

        Instantiate(pickUp.itemIconForMainSlot, placeItemIconForMainSlot);
    }

    public void ShowHand()
    {
        emptyHand.SetActive(true);
    }

    public void RemoveItem()
    {
        if (!isFull)
        {
			return;
        }
        Inventory.SubFromTotalWeight(pickUp.itemWeigth);
        ItemThrowOnScene();
        ShowHand();

        ClearSlot();
    }

    public void ClearSlot()
    {

        Destroy(placeItemIconForMainSlot.transform.GetChild(0).gameObject);

        pickUp = null;
        isFull = false;
    }

    public void PutCurrentItemToInventory()
    {
        if (!isFull)
        {
            return;
        }

        Inventory.PutInInventory(pickUp);
        Inventory.SubFromTotalWeight(pickUp.itemWeigth);
    }

    public void ButtonPutCurrentItemToInventory()
    {
        if (!isFull)
        {
            return;
        }

        Weapon.SetWeapon(Weapon.WeaponType.NONE);
        ShowHand();

        Inventory.PutInInventory(pickUp);
        Inventory.SubFromTotalWeight(pickUp.itemWeigth);

        ClearSlot();
    }


    private void ItemThrowOnScene()
    {
        Weapon.weapons[(int)pickUp.weaponType].isInInventory = false;

        Weapon.SetWeapon(Weapon.WeaponType.NONE);

        //TODO включить пустые руки

        GameObject clone = Instantiate(pickUp.gameObject, Inventory.placeItem.position
        + new Vector3(1, 0, 0), Quaternion.identity);
        clone.SetActive(true);
    }

}


