using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperPanel : MonoBehaviour
{
    public Transform Logo, HelpPanel;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            Logo.gameObject.SetActive(!Logo.gameObject.activeSelf);
            HelpPanel.gameObject.SetActive(!HelpPanel.gameObject.activeSelf);
        }
        else if (Input.anyKeyDown)
        {
            Logo.gameObject.SetActive(false);
            HelpPanel.gameObject.SetActive(false);
        }
    }
}
