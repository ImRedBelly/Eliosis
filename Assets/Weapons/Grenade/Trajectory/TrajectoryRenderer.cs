using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject grenadePrefab;
    GameObject grenade;

    private Dictionary<Rigidbody2D, BodyData> saveBodies = new Dictionary<Rigidbody2D, BodyData>();

    Vector3[] maxPoints = new Vector3[100];
    private void Start()
    {
        foreach (var rb in FindObjectsOfType<Rigidbody2D>())
        {
            saveBodies.Add(rb, new BodyData());
        }
    }
    public void AddBody(Rigidbody2D rb)
    {
        saveBodies.Add(rb, new BodyData());
    }
    public void RemoveBody(Rigidbody2D rb)
    {
        saveBodies.Remove(rb);
    }
    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        foreach (var body in saveBodies)
        {
            body.Value.position = body.Key.transform.position;
            body.Value.rotation = body.Key.transform.rotation;
            body.Value.velocity = body.Key.velocity;
            body.Value.angularVelocity = body.Key.angularVelocity;
        }



        grenade = Instantiate(grenadePrefab, origin, Quaternion.identity);
        grenade.GetComponent<Rigidbody2D>().AddForce(speed, ForceMode2D.Impulse);

        Physics2D.autoSimulation = false;

        lineRenderer.positionCount = 0;
        lineRenderer.positionCount = maxPoints.Length;

        for (int i = 0; i < maxPoints.Length; i++)
        {
            Physics2D.Simulate(Time.fixedDeltaTime);
            if (i < 5)
            {
                grenade.GetComponent<CapsuleCollider2D>().isTrigger = false;
            }
            maxPoints[i] = grenade.transform.position;
        }
        lineRenderer.SetPositions(maxPoints);

        Physics2D.autoSimulation = true;

        foreach (var body in saveBodies)
        {
            body.Key.transform.position = body.Value.position;
            body.Key.transform.rotation = body.Value.rotation;
            body.Key.velocity = body.Value.velocity;
            body.Key.angularVelocity = body.Value.angularVelocity;
        }



        Destroy(grenade);
    }
    public class BodyData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public float angularVelocity;
    }
}
