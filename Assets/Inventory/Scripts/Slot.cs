using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	public bool isFull;

	public GameObject placeForIcon;
	public Text textName;
	public Text textWeigth;
	public Button buttonApply;
	public Button buttonRemove;

	public int numberOfItems;
	public Text textNumberOfItems;

	PickUp pickUp;

	Inventory inventory;
	MainSlot mainSlot;


	private void Start()
	{
		inventory = FindObjectOfType<Inventory>();

		mainSlot = FindObjectOfType<MainSlot>();

		textNumberOfItems.gameObject.SetActive(false);
	}



public void AddItem(PickUp pickUp)
	{
		this.pickUp = pickUp;

		isFull = true;

		AddItem();

		Instantiate(pickUp.itemIcon, placeForIcon.transform);
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

		if (numberOfItems > 1)
		{
			textNumberOfItems.gameObject.SetActive(true);
		}
		
		textNumberOfItems.text = $"х {numberOfItems}";
	}

	void SubNumberOfItems()
	{
		--numberOfItems;

		if (numberOfItems <= 1)
		{
			textNumberOfItems.gameObject.SetActive(false);
		}

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

		GameObject clone = Instantiate(pickUp.item, inventory.transform.position
						   + new Vector3(1, 0, 0), Quaternion.identity);

		clone.SetActive(true);

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
        if (!mainSlot.isFull)
        {
			mainSlot.AddItem(pickUp);
			return;
		}

		mainSlot.PutCurrentItemToInventory();
		mainSlot.ClearSlot();
		mainSlot.AddItem(pickUp);
	}

	private void ThrowOnScene()
	{
		pickUp.gameObject.transform.SetParent(null);
		pickUp.gameObject.transform.position = inventory.transform.position + new Vector3(1, 0, 0);
		pickUp.gameObject.SetActive(true);
	}


}


