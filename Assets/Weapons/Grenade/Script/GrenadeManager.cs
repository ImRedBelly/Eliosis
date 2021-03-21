using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public TrajectoryRenderer trajectoryRenderer;
    public GameObject grenadePrefab;

    GameObject grenade;

    void Update()
    {
        float enter;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
        Vector3 mouseInWorld = ray.GetPoint(enter);

        Vector3 speed = (mouseInWorld - transform.position);



        if (Input.GetKey(KeyCode.G))
        {
            trajectoryRenderer.gameObject.SetActive(true);
            trajectoryRenderer.ShowTrajectory(transform.position, speed);
        }


        if (Input.GetKeyDown(KeyCode.G))
        {

        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            trajectoryRenderer.gameObject.SetActive(false);
            grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            grenade.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);
        }
    }  
    
}
