using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    public Transform[] points;
    private Vector3[] waypoints;
    public float speed;
    private int currentWaypoint;
    Rigidbody2D rb;

    bool isCountDown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        waypoints = new Vector3[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            waypoints[i] = points[i].position;
        }
    }


    void FixedUpdate ()
    {

        if (waypoints == null || waypoints.Length <= 1)
        {
            return;
        }



        if (Vector3.Distance(transform.position, waypoints[currentWaypoint]) < 0.1f)
        {

            if (isCountDown)
            {
                currentWaypoint--;
            }
            else
            {
                currentWaypoint++;
            }


            if (currentWaypoint > waypoints.Length - 1)
            {
                currentWaypoint--;
                isCountDown = true;
            }
            else if (currentWaypoint == 0)
            {
                isCountDown = false;
            }

        }

        Vector3 direction = waypoints[currentWaypoint] - transform.position;
        direction.z = 0;
        rb.velocity = direction.normalized * speed;

    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }
}
