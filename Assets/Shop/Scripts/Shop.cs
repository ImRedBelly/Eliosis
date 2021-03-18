using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject menuShop;

    public Text moneyPlayer;

    public Text priceSpeed;
    public Text priceHealth;
    public Text priceDamage;

    public int maxPriceSpeed;
    public int maxPriceHealth;
    public int maxPriceDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            menuShop.SetActive(true);
            ShopOn();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            menuShop.SetActive(false);
            ShopOff();
        }

    }


    public void CloseShop()
    {
        menuShop.SetActive(false);
        print("close");
    }
    public void ShopOn()
    {
        moneyPlayer.text = (Purse.instance.money).ToString();

        priceSpeed.text = maxPriceSpeed + " $";
        priceHealth.text = maxPriceHealth + " $";
        priceDamage.text = maxPriceDamage + " $";
    }
    void ShopOff()
    {

    }




    public void BuySpeed()
    {
        maxPriceSpeed = Purse.instance.Spend(maxPriceSpeed);
        ShopOn();

    }
    public void BuyHealth()
    {
        maxPriceHealth = Purse.instance.Spend(maxPriceHealth);
        if (Purse.instance.Buy())
        {
            HealthPlayer health = FindObjectOfType<HealthPlayer>();
            health.UpdateMaxHealth(1);
        }
        ShopOn();
    }
    public void BuyDamage()
    {
        maxPriceDamage = Purse.instance.Spend(maxPriceDamage);
        if (Purse.instance.Buy())
        {
            Attack attack = FindObjectOfType<Attack>();
            attack.UpdateMaxDamage(1);
        }

        ShopOn();
    }
}

