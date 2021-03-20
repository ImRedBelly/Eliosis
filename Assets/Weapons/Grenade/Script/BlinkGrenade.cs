using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkGrenade : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Color ColorA, ColorB;
    public float Rate;

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        sprite.color = Color.Lerp(ColorA, ColorB, Mathf.Abs(Mathf.Sin(time * Rate)));
    }
}
