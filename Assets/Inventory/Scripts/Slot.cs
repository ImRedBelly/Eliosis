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
	Inventory Inventory {
		get
		{
			if (inventory == null)
			{
				inventory = PlayerMovement.instance.inventory;
			}
			return inventory;
		} 
	}

	MainSlot MainSlot { 
		get
		{
			if (mainSlot == null)
			{
				mainSlot = FindObjectOfType<MainSlot>();
			}
			return mainSlot;
		} 
	}

public void AddItem(PickUp pickUp)
	{
		print("add item");
		print(pickUp.gameObject);
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

	private void OnEnable()
	{

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
			ClearSlot();
		}
		
	}

    private void ClearSlot()
    {
		isFull = false;

		Destroy(placeForIcon.transform.GetChild(0).gameObject);

		textName.text = "";
		textWeigth.text = "";

		pickUp = null;
	}



    public void ApplyItem()
	{
		if (!isFull)
		{
			return;
		}

		SubNumberOfItems();

		pickUp.ApplyPickUp();

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
		GameObject clone = Instantiate(pickUp.item, inventory.placeItem.position
		+ new Vector3(1, 0, 0), Quaternion.identity);
		clone.SetActive(true);
	}

}


