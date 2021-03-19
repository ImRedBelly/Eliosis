using UnityEngine;

public class Purse : MonoBehaviour
{
    public static Purse instance;
    public int money;
    bool isBuy = false;
    void Start()
    {
        instance = this;
    }

    public int Spend(int price)
    {
        if (money <= 0)
        {
            isBuy = false;
            //print("Денег нет");
        }
        else
        {
            // print("Деньги есть");

            if ((money - price) < 0)
            {
                isBuy = false;
                // print("Не хватает");
            }
            else
            {
                //print("Денег хватает");
                isBuy = true;
                money = money - price;
                price++;
            }
        }
        return price;
    }

    public bool Buy()
    {
        return isBuy;
    }

    public void GiveMoney()
    {
        GameManager.instance.GiveMoney(money);
    }
    public void TakeMoney(int ghostMoney)
    {
        money += ghostMoney;
    }
}
