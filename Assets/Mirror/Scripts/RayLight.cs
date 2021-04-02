using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLight : MonoBehaviour
{
	[SerializeField] private LayerMask whatIsObstacles;

	Vector2 startPoint, direction;
	List<Vector3> points = new List<Vector3>();
	int distance = 100;
	LineRenderer lr;


	private float timeCounter;
	private float timeChangeSprite = 0.1f;
	public Texture[] textures;
	private int animationStep;

    private void Awake()
    {
		lr = GetComponent<LineRenderer>();
	}

    void Start()
	{
		lr.enabled = true;
		startPoint = transform.position;
		direction = -transform.up;
	}

	private void Update()
	{
		RayAnimation();

		var hitData = Physics2D.Raycast(startPoint, direction, distance, whatIsObstacles);

		points.Clear();
		points.Add(startPoint);

		if (hitData)
		{

			if (!hitData.collider.CompareTag("Mirror"))
			{
				points.Add(hitData.point);
				lr.positionCount = points.Count;
				lr.SetPositions(points.ToArray());
				return;
			}

			ReflectFurther(startPoint, hitData);
		}
		else
		{
			points.Add(startPoint + direction * distance);
		}

		lr.positionCount = points.Count;
		lr.SetPositions(points.ToArray());
	}

	private void ReflectFurther(Vector2 origin, RaycastHit2D hitData)
	{

		points.Add(hitData.point);

		Vector2 inDirection = (hitData.point - origin).normalized;
		Vector2 newDirection = Vector2.Reflect(inDirection, hitData.normal);

		var newHitData = Physics2D.Raycast(hitData.point + (newDirection * 0.001f), newDirection, distance, whatIsObstacles);
		if (newHitData)
		{
			if (!newHitData.collider.CompareTag("Mirror")) 
			{
				points.Add(newHitData.point);
				return;
			}

			ReflectFurther(hitData.point, newHitData);

		}
		else
		{
			points.Add(hitData.point + newDirection * distance);
		}
	}

	private void RayAnimation()
	{
		timeCounter += Time.deltaTime;

		if (timeCounter >= timeChangeSprite)
		{
			animationStep++;
			if (animationStep == textures.Length)
			{
				animationStep = 0;
			}

			lr.material.SetTexture("_MainTex", textures[animationStep]);

			timeCounter = 0f;
		}
	}
}



