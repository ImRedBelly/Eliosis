using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketMovement : MonoBehaviour
{
    public float speed = 10f;

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

    GameObject rocket;

    ValueManagerPlayer valuePlayer;

    public float damageValue = 5;
    public float power = 50;

    public Camera cam;

    AudioSource audioSource;
    public AudioClip fly;
    public AudioClip boom;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        cam = Camera.main;
        valuePlayer = ValueManagerPlayer.instance;
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isCanStarted)
        {
            // TODO надо изменить курсор
        }


        if (Input.GetKey(KeyCode.X) && !isCanStarted)
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

                GameObject cloneText = Instantiate(prefabAimRocket, points[pointIndex], Quaternion.identity);
                pointsAim.Add(cloneText);

                cloneText.GetComponent<Text>().text = (pointIndex + 1).ToString();

                if (pointIndex == pointsNumbers - 2)
                {
                    cloneText.GetComponent<Text>().color = Color.red;
                    cloneText.GetComponentInChildren<Image>().color = Color.red;
                }
                else
                {
                    cloneText.GetComponent<Text>().color = Color.cyan;
                    cloneText.GetComponentInChildren<Image>().color = Color.cyan;
                }

                pointIndex++;

            }
        }

        if (Input.GetKeyUp(KeyCode.X) && !isCanStarted)
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
            audioSource.PlayOneShot(boom);
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
    }

    void Move()
    {
        if (pointIndex == 0)
        {
            audioSource.PlayOneShot(fly);
        }

        SetPath();

        if (isCanFinished)
        {
            return;
        }

        float distanceCurrent = Vector2.Distance(rocket.transform.position, nextPoint);


        dir = nextPoint - currentPoint;
        rocket.transform.right = dir;
        rocket.transform.position = Vector2.MoveTowards(rocket.transform.position, nextPoint, Time.deltaTime * speed);


        if (distanceCurrent < 1f)
        {

              Destroy(pointsAim[pointIndex]);

        }

        if (distanceCurrent < 0.2f)
        {
            pointIndex++;
            audioSource.PlayOneShot(fly);
        }


    }

    void SetPath()
    {
        if (pointIndex == pointsNumbers-1)
        {
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
