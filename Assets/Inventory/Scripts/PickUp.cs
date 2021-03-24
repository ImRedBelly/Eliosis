using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField] public GameObject item;
    [SerializeField] public GameObject itemUsedOnPlayer; // для главного слота
    [SerializeField] public GameObject itemIcon;
    [SerializeField] public bool isOneOff; // например, аптечка. Оружие -нет.
    [SerializeField] public string itemName;
    [SerializeField] public int itemWeigth;


    [SerializeField] private GameObject Glow;

    private bool isTriggered;

    private Inventory inventory;

    private Transform appliedInventoryPlace;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        Glow.SetActive(false);
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L) && isTriggered)
        {
            inventory.PutInInventory(this);
        }
    }

    public void ApplyPickUp() // у каждого свой метод
    {
        foreach (Transform child in appliedInventoryPlace)
        {
            if (child.gameObject)
            {
                Destroy(child.gameObject);
            }          
        }
        Instantiate(itemUsedOnPlayer, itemUsedOnPlayer.transform);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isTriggered = true;
            PossibilityToTake(isTriggered);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = false;
            PossibilityToTake(isTriggered);
        }
    }

    private void PossibilityToTake(bool enable)
    {
        Glow.SetActive(enable);
    }

}
