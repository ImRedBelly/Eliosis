using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float paralaxEffect;

    public bool isUseCollider;

    void Start()
    {
        startPos = transform.position.x;

        if (isUseCollider)
        {
            length = GetComponent<Collider2D>().bounds.size.x;
            return;
        }

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = cam.transform.position.x * (1 - paralaxEffect);
        float dist = cam.transform.position.x * paralaxEffect;

        // двигаем фон с поправкой на paralaxEffect
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // если камера перескочила спрайт, то меняем startPos
        if (temp > startPos + length)
            startPos += length;
        else if (temp < startPos - length)
            startPos -= length;
    }
}