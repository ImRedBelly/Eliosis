using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public AudioClip shell;
    AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartCoroutine(SoundShell());
        Destroy(gameObject, 3f);
    }
    IEnumerator SoundShell()
    {
        yield return new WaitForSeconds(0.4f);
        audioSource.PlayOneShot(shell);
    }
}
