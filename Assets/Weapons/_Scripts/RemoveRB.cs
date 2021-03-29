using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRB : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        TrajectoryRenderer.instance.RemoveBody(rb);
    }
}
