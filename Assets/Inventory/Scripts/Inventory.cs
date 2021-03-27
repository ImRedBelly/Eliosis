using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public Transform placeItem;
	public Slot[] slots;
	public MainSlot mainSlot;
	public Text textTotalWeight;
	public int totalWeight; // вес для Димы


	private void Start()
	{
		totalWeight = 0;
		textTotalWeight.text = $"TOTAL: {totalWeight}";

		gameObject.SetActive(false);	
	}

    public void PutInInventory(PickUp pickUp)
	{
		for (int i = 0; i < slots.Length; i++) // поработать над условием
		{
			if (slots[i].isFull && (slots[i].textName.text == pickUp.itemName))
			{
				slots[i].AddItem();
				return;
			}
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].isFull)
			{
				slots[i].AddItem(pickUp);

				slots[i].gameObject.SetActive(true);

				break;
			}
		}
	}


	public void AddToTotalWeight(int weigth)
	{
		totalWeight += weigth;
		textTotalWeight.text = $"TOTAL: {totalWeight}";
	}

	public void SubFromTotalWeight(int weigth)
	{
		totalWeight -= weigth;
		textTotalWeight.text = $"TOTAL: {totalWeight}";
	}
}

