using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject portal;
    public int torchCount;

    public void CreateTorch()
    {
        torchCount++;
    }
    public void Light()
    {
        torchCount--;
        if (torchCount <= 0)
        {
            portal.SetActive(true);
        }
    }

}
