using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public GameObject inventoryPanel;
	public Transform placeItem;


	public Slot[] slots;
	public Text textTotalWeight;
	public int totalWeight;




	private void Start()
	{
		totalWeight = 0;
		textTotalWeight.text = $"TOTAL: {totalWeight}";

		inventoryPanel.SetActive(false);

	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
			inventoryPanel.SetActive(!inventoryPanel.activeSelf);
		}
    }

    public void PutInInventory(PickUp pickUp)
	{


		for (int i = 0; i < slots.Length; i++) // поработать над условием
		{
			if (slots[i].isFull && (slots[i].textName.text == pickUp.itemName)) // если такой pickup уже есть
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

