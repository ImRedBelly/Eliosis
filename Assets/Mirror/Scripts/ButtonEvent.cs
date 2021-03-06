using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
	public UnityEvent OnPressed;
	public UnityEvent OnUnPressed;

    public GameObject platform;

	Animator animator;

    private void Awake()
    {
		animator = GetComponent<Animator>();

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPressed.Invoke();
            animator.SetBool("Pressed", true);
        }
        if (platform == null)
        {
            return;
        }
        collision.transform.SetParent(platform.transform);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnUnPressed.Invoke();
            animator.SetBool("Pressed", false);
        }

        if (platform == null)
        {
            return;
        }

        collision.transform.SetParent(null);

    }

}