using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public Transform ahchor;

    float distanceHook = 15f;
    float stopping = 0.15f;

    [SerializeField] private LayerMask whatIsHooking;

    [SerializeField] private LineRenderer lineHook;
    [SerializeField] private Texture[] textures;
    [SerializeField] private float timeChangeSprite;
    private float timeCounter;
    private int animationStep;

    DistanceJoint2D joint;


    private void Awake()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        lineHook.enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 mouseTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseTarget.z = 0;
            RaycastHit2D rayCast =
            Physics2D.Raycast(ahchor.position, mouseTarget - ahchor.position, distanceHook, whatIsHooking);


            if (rayCast.collider != null)
            {
                joint.enabled = true;
                lineHook.enabled = true;

                joint.connectedBody = rayCast.collider.attachedRigidbody;
                joint.connectedAnchor = rayCast.point - (Vector2)rayCast.collider.transform.position;

                joint.distance = Vector2.Distance(ahchor.position, rayCast.point);

                lineHook.SetPosition(0, ahchor.position);
                lineHook.SetPosition(1, rayCast.point);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            lineHook.SetPosition(0, ahchor.position);


            if (joint.distance > 3f)
            {
                joint.distance -= stopping;
                HookAnimation();
            }
            else
            {
                joint.enabled = false;
                lineHook.enabled = false;
            }


        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            joint.enabled = false;
            lineHook.enabled = false;
        }
    }

    private void HookAnimation()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeChangeSprite)
        {
            animationStep++;
            if (animationStep == textures.Length)
            {
                animationStep = 0;
            }

            lineHook.material.SetTexture("_MainTex", textures[animationStep]);

            timeCounter = 0f;
        }
    }

}
