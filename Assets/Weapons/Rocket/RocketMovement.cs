using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketMovement : MonoBehaviour
{
    public int pointsNumbers;

    List<Vector2> points = new List<Vector2>();

    List<GameObject> pointsAim = new List<GameObject>();

    int pointIndex = 0;

    Vector2 currentPoint;
    Vector2 nextPoint;

    bool isCanStarted;
    private bool isCanMove;
    private bool isCanFinished;

    Vector2 dir;

    public Transform placeRocket;
    public GameObject prefabAimRocket;
    public GameObject prefabRocket;
    public GameObject prefabBoom;

    Rigidbody2D rb;
    GameObject rocket;

    ValueManagerPlayer valuePlayer;

    public float damageValue = 5;
    public float power = 50;

    public Camera cam;

    private void Start()
    {
        cam = Camera.main;

        valuePlayer = ValueManagerPlayer.instance;
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isCanStarted)
        {
            // TODO надо изменить курсор
        }


        if (Input.GetKey(KeyCode.R) && !isCanStarted)
        {

            if (Input.GetMouseButtonDown(0))
            {

                if (pointIndex == pointsNumbers - 1)
                {

                    points.Insert(0, placeRocket.position);
                    isCanStarted = true;
                    return;
                }


                points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                GameObject clonText = Instantiate(prefabAimRocket, points[pointIndex], Quaternion.identity);
                pointsAim.Add(clonText);

                clonText.GetComponent<Text>().text = (pointIndex + 1).ToString();

                if (pointIndex == pointsNumbers - 2)
                {
                    clonText.GetComponent<Text>().color = Color.red;
                    clonText.GetComponentInChildren<Image>().color = Color.red;
                }
                else
                {
                    clonText.GetComponent<Text>().color = Color.cyan;
                    clonText.GetComponentInChildren<Image>().color = Color.cyan;
                }

                pointIndex++;

            }
        }

        if (Input.GetKeyUp(KeyCode.R) && !isCanStarted)
        {

            if (isCanMove  &&  !isCanFinished)
            {
                // TODO надо вернуть курсор

                return;
            }



            foreach (var item in pointsAim)
            {
                Destroy(item);
            }
            points.Clear();
            pointsAim.Clear();

            isCanStarted = false;

            pointIndex = 0;
        }

        if (isCanStarted)
        {
            pointIndex = 0;

            isCanStarted = false;

            isCanMove = true;

            CreatRocket();

        }

        if (isCanMove)
        {
            Move();
        }

        if (isCanFinished)
        {
            points.Clear();
            pointsAim.Clear();
            pointIndex = 0;

            Instantiate(prefabBoom, rocket.transform.position, Quaternion.identity);
            DoDashDamage();

            Destroy(rocket);

            isCanStarted = false;
            isCanMove = false;
            isCanFinished = false;

        }


    }

    private void CreatRocket()
    {
        rocket = Instantiate(prefabRocket, placeRocket.position, Quaternion.identity);
        rb = rocket.GetComponent<Rigidbody2D>();
    }

    void Move()
    {
        SetPath();

        if (isCanFinished)
        {
            return;
        }

        float distanceCurrent = Vector2.Distance(rocket.transform.position, nextPoint);


        dir = nextPoint - currentPoint;
        rocket.transform.right = dir;
        rb.velocity = dir.normalized * 8f;


        if (distanceCurrent < 1f)
        {

              Destroy(pointsAim[pointIndex]);

        }

        if (distanceCurrent < 0.2f)
        {
            pointIndex++;
        }


    }

    void SetPath()
    {
        if (pointIndex == pointsNumbers-1)
        {
            rb.velocity = Vector2.zero;
            isCanMove = false;
            isCanFinished = true;
            return;
        }

        currentPoint = points[pointIndex];
        nextPoint = points[pointIndex + 1];

    }



    void DoDashDamage()
    {
        damageValue = Mathf.Abs(damageValue);
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(rocket.transform.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Enemy" || collidersEnemies[i].gameObject.tag == "DeathCopy")
            {
                if (collidersEnemies[i].transform.position.x - rocket.transform.position.x < 0)
                {
                    damageValue = -damageValue;
                }
                collidersEnemies[i].gameObject.SendMessage("ApplyDamage", valuePlayer.bulletValue.damageBullet * 3);
                cam.GetComponent<CameraFollow>().ShakeCamera();
            }
        }
    }

    public void DamageOnTrigger()
    {
        Instantiate(prefabBoom, rocket.transform.position, Quaternion.identity);
        DoDashDamage();
        Destroy(rocket);

        foreach (var item in pointsAim)
        {
            Destroy(item);
        }
        pointIndex = 0;
        points.Clear();
        pointsAim.Clear();

        isCanStarted = false;
        isCanMove = false;
        isCanFinished = false;
    }

}
