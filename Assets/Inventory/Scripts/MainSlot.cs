using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSlot : MonoBehaviour
{
	public bool isFull;

	public Transform placeItemIconForMainSlot;

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
        Instantiate(pickUp.itemIconForMainSlot, placeItemIconForMainSlot);
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

        inventory.PutInInventory(pickUp);
        inventory.SubFromTotalWeight(pickUp.itemWeigth);
        ClearSlot();
    }


    private void ItemThrowOnScene()
    {
        GameObject clone = Instantiate(pickUp.item, inventory.placeItem.position
        + new Vector3(1, 0, 0), Quaternion.identity);
        clone.SetActive(true);
    }

}


