using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversLookDouble : MonoBehaviour
{
	Vector2 mousePosition;

	void Update()
	{
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (mousePosition.x > PlayerMovement.instance.transform.position.x)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180);
		}
		else
		{
			transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 180);
		}

	}
}
