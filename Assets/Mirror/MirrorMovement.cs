using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    private bool isEnable;


    void Update()
    {
        if (isEnable)
        {
            RotateMirror();        
        }
        print(isEnable);

    }

    private void RotateMirror()
    {
        transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * 20f);
    }

    public void EnableRotation(bool enabled)
    {
        isEnable = enabled;
    }

}
