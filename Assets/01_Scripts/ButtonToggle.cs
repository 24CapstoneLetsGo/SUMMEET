using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    private bool isActive = false;

    public void OnButtonToggle()
    {
        isActive = isActive ? false : true;
        gameObject.SetActive(isActive);
    }
}
