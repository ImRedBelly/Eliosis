using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent <Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        print(col.bounds.size);
    }
}
