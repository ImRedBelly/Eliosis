using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    void Update()
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        transform.position = PlayerMovement.instance.transform.position;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
            PlayerMovement.instance.transform.localScale.x * PlayerMovement.instance.transform.localScale.x);
    }
    private void Rotate()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction.z = 0;

        float angle = Vector2.SignedAngle(PlayerMovement.instance.transform.up, direction);
        if (angle > 90)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        if (angle < -90)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            return;
        }
        transform.right = -direction;
    }

}
