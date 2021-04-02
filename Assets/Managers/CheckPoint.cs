using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public float sizeCamera = 10;
    float startSizeCamera;
    bool isTrigg = false;
    void Update()
    {
        if (isTrigg)
        {
            startSizeCamera = Mathf.Lerp(startSizeCamera, sizeCamera, Time.deltaTime);

            Camera.main.orthographicSize = startSizeCamera;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DeathController deathController = FindObjectOfType<DeathController>();
            deathController.checkPoint = transform;
            startSizeCamera = Camera.main.orthographicSize;
            isTrigg = true;
        }
    }
}
