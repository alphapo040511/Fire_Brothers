using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocus : MonoBehaviour
{
    public GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    private void OnDisable()
    {
        if(EventSystem.current != null)
        EventSystem.current.SetSelectedGameObject(null);
    }
}
