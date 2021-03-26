using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject light;
    public SpriteRenderer button;
    public Sprite buttonOn;
    LevelManager levelManager;
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        levelManager.CreateTorch();
    }


    public void Fire()
    {
        light.GetComponent<BlinkGrenade>().enabled = true;
        button.sprite = buttonOn;
        levelManager.Light();
    }

}
