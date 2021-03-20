using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public TrajectoryRenderer trajectoryRenderer;
    public GameObject grenadePrefab;
    public GameObject effectBoom;
    public float power = 50;

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
            StartCoroutine(IsTrigger(grenade.gameObject));
            StartCoroutine(Boom());
        }
    }
    IEnumerator Boom()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(effectBoom, grenade.transform.position, Quaternion.identity);
        Destroy(grenade);
    }
    IEnumerator IsTrigger(GameObject grenade)
    {
        grenade.GetComponent<CapsuleCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.1f);
        grenade.GetComponent<CapsuleCollider2D>().isTrigger = false;
    }
}
