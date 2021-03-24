using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSlot : MonoBehaviour
{
	public bool isFull;

	public Image imagePlayerWithItem;
	public Button buttonRemove;

	Inventory inventory;
    PickUp pickUp;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void AddItem(PickUp pickUp)
	{
        isFull = true;

        this.pickUp = pickUp;
		imagePlayerWithItem.sprite = pickUp.itemUsedOnPlayer.GetComponent<SpriteRenderer>().sprite;
	}

    public void RemoveItem()
    {
        if (!isFull)
        {
			return;
        }
        inventory.SubFromTotalWeight(pickUp.itemWeigth);
        ItemThrowOnScene();
        ClearSlot();
    }

    public void ClearSlot()
    {
        imagePlayerWithItem.sprite = null;
        pickUp = null;
        isFull = false;
    }

    public void PutCurrentItemToInventory()
    {
        if (!isFull)
        {
            return;
        }

        inventory.PutInInventory(pickUp);
        inventory.SubFromTotalWeight(pickUp.itemWeigth);
        ClearSlot();
    }


    private void ItemThrowOnScene()
    {
        pickUp.gameObject.transform.SetParent(null);
        pickUp.gameObject.transform.position = inventory.transform.position + new Vector3(1, 0, 0);
        pickUp.gameObject.SetActive(true);
    }

}


