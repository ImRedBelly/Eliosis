using UnityEngine;

public class ReversLook : MonoBehaviour
{
    public GameObject reversObject;
    Vector2 mousePosition;
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x > PlayerMovement.instance.transform.position.x)
            reversObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        else
            reversObject.transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
    }
}
