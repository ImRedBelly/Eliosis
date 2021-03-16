using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateBehaviour : StateMachineBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioJump;
    public AudioClip audioLand;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsJumping") == true)
        {
            animator.SetBool("IsJumping", false);
            audioSource = animator.transform.GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.clip = audioJump;
                audioSource.Play();
            }

        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsJumping", false);
    }
}
