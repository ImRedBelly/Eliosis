using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
	public UnityEvent OnPressed;
	public UnityEvent OnUnPressed;

	Animator animator;

    private void Awake()
    {
		animator = GetComponent<Animator>();

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
		OnPressed.Invoke();
		animator.SetBool("Pressed", true);
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		OnUnPressed.Invoke();
		animator.SetBool("Pressed", false);
	}

}