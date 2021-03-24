using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public Slot[] slots;
	public Text textTotalWeight;
	public int totalWeight;


	private void Start()
	{
		totalWeight = 0;
		textTotalWeight.text = $"Общий вес: {totalWeight}";
	}

	public void PutInInventory(PickUp pickUp)
	{
		for (int i = 0; i < slots.Length; i++) // поработать над условием
		{
			if (slots[i].isFull && (slots[i].textName.text == pickUp.itemName)) // если такой pickup уже есть
			{
				slots[i].AddItem();

				pickUp.gameObject.transform.SetParent(transform);
				pickUp.gameObject.transform.localPosition = Vector3.zero;
				pickUp.gameObject.SetActive(false);

				return;
			}
		}

		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i].isFull)
			{
				slots[i].AddItem(pickUp);

				pickUp.gameObject.transform.SetParent(transform);
				pickUp.gameObject.transform.localPosition = Vector3.zero;
				pickUp.gameObject.SetActive(false);

				break;
			}
		}
	}


	public void AddToTotalWeight(int weigth)
	{
		totalWeight += weigth;
		textTotalWeight.text = $"Общий вес: {totalWeight}";
	}

	public void SubFromTotalWeight(int weigth)
	{
		totalWeight -= weigth;
		textTotalWeight.text = $"Общий вес: {totalWeight}";
	}
}

