using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int countDeath;
    public int countMoney;

    public float PositionX;
    public float PositionY;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            Time.timeScale += Time.deltaTime * 3;
            Time.timeScale = Mathf.Clamp01(Time.timeScale);
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            Time.timeScale -= Time.deltaTime * 3;
            Time.timeScale = Mathf.Clamp01(Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
    }
    public void SavePosition()
    {
        PlayerMovement.instance.GetComponent<Purse>().GiveMoney();

        PositionX = PlayerMovement.instance.transform.position.x;
        PositionY = PlayerMovement.instance.transform.position.y;
    }
    public void GiveMoney(int money)
    {
        countMoney += money;
    }

}
