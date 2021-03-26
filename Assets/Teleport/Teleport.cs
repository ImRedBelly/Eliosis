using System.Collections;
using UnityEngine;



public class Teleport : MonoBehaviour
{
    public GameObject teleport;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Portal();
            }
        }
    }
    void Portal()
    {
        PlayerMovement.instance.transform.position = teleport.transform.position;
    }
}
