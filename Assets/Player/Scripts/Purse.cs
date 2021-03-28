using UnityEngine;

public class Purse : MonoBehaviour
{
    public AudioClip goodBuy;
    public AudioClip badBuy;
    AudioSource audioSource;
    public static Purse instance;
    public int money;
    bool isBuy = false;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public int Spend(int price)
    {
        if (money <= 0)
        {
            isBuy = false;
            audioSource.PlayOneShot(badBuy);
            //print("Денег нет");
        }
        else
        {
            // print("Деньги есть");

            if ((money - price) < 0)
            {
                isBuy = false;
                audioSource.PlayOneShot(badBuy);
                // print("Не хватает");
            }
            else
            {
                //print("Денег хватает");
                isBuy = true;
                audioSource.PlayOneShot(goodBuy);
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
        money = 0;
    }
    public void TakeMoney(int ghostMoney)
    {
        money += ghostMoney;
    }
}
