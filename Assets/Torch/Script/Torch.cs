using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject light;
    public SpriteRenderer button;
    public Sprite buttonOn;

    LevelManager levelManager;
    bool isFire;
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.CreateTorch();
        isFire = true;
    }


    public void Fire()
    {
        if (isFire)
        {
            light.GetComponent<BlinkGrenade>().enabled = true;
            button.sprite = buttonOn;
            levelManager.Light();
            isFire = false;
        }
    }
}


