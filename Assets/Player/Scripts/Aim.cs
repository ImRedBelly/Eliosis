using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public bool isUseHardwareCursor;

    public float distanceFromPlayer = 2f;

    public GameObject prefabCursor;
    GameObject cursor;
    SpriteRenderer sr;


    void Awake()
    {
        if (isUseHardwareCursor)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            return;
        }
        Cursor.visible = false;
    }

    private void Start()
    {
        cursor = Instantiate(prefabCursor);
    }

    private void Update()
    {
        if (isUseHardwareCursor)
        {
            return;
        }
        ArroundPlayer();
    }

    void ArroundPlayer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;

        cursor.transform.position = transform.position + direction.normalized * distanceFromPlayer;
    }
}
