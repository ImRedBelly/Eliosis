using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject tower;

    public GameObject bullet;
    public GameObject flashPrefab;
    public GameObject shotPosition;

    bool isPlayer = false;


    public GameObject player;
    private Vector2 distToPlayer;

    public SpriteRenderer angleSprite;

    public float distanceToPlayer;

    public float[] patrolAngles;

    float timeToLerp;
    float timeToShot;


    public GameObject[] deadEffect;
    float health = 3;
    bool isDestroy = true;
    public AudioSource audioSource;
    public AudioClip shoot;
    void Start()
    {
        player = GameObject.Find("Player _Yura");
    }


    void Update()
    {
        distToPlayer = player.transform.position - tower.transform.position;

        Vector3 targetDir = player.transform.position - tower.transform.position;
        float angle = Vector3.Angle(targetDir, -tower.transform.up);

        if (distToPlayer.magnitude < distanceToPlayer && angle > patrolAngles[0] && angle < patrolAngles[1])
        {
            isPlayer = true;

            Color colorAngle = Color.red;
            colorAngle.a = 0.5f;
            angleSprite.color = colorAngle;
        }
        else
        {
            isPlayer = false;

            Color colorAngle = Color.green;
            colorAngle.a = 0.5f;
            angleSprite.color = colorAngle;
        }

        if (isPlayer)
        {
            ShotInPlayer();

            Vector2 directionToPlayerOffLerp = tower.transform.position - player.transform.position;
            tower.transform.up = directionToPlayerOffLerp;
        }
        else
        {
            timeToLerp += Time.deltaTime;
            tower.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, patrolAngles[0]), Quaternion.Euler(0, 0, patrolAngles[1]), Mathf.Abs(Mathf.Sin(timeToLerp * 0.5f)));
        }
        if (health <= 0)
        {
            DestroyTurret();
        }
    }


    void ShotInPlayer()
    {
        if (timeToShot < 0)
        {
            audioSource.PlayOneShot(shoot);

            GameObject bulletCopy = Instantiate(bullet, shotPosition.transform.position, Quaternion.identity);
            bulletCopy.transform.right = -tower.transform.up;

            bulletCopy.GetComponent<Bullet>().direction = shotPosition.transform.up;
            bulletCopy.gameObject.layer = LayerMask.NameToLayer("BulletEnemy");



            GameObject flash = Instantiate(flashPrefab, shotPosition.transform.position, Quaternion.identity);
            flash.transform.right = -tower.transform.up;
            Destroy(flash, 1f);


            timeToShot = 4;
        }
        else
        {
            timeToShot -= Time.deltaTime;
        }
    }
    void DestroyTurret()
    {
        if (isDestroy)
        {
            Instantiate(deadEffect[0], transform.position, Quaternion.identity);
            Instantiate(deadEffect[1], transform.position, Quaternion.identity);
            isDestroy = false;
        }
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
        {
            health--;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 lookDirection = -tower.transform.up;
        Vector3 lookLeftDirection = Quaternion.AngleAxis(patrolAngles[0], Vector3.forward) * lookDirection;
        Vector3 lookRightDirection = Quaternion.AngleAxis(patrolAngles[1], Vector3.forward) * lookDirection;

        Gizmos.DrawRay(tower.transform.position, lookDirection * distanceToPlayer);
        Gizmos.DrawRay(tower.transform.position, lookLeftDirection * distanceToPlayer);
        Gizmos.DrawRay(tower.transform.position, lookRightDirection * distanceToPlayer);
    }
}
