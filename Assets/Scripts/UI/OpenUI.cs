using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    public ScreenType ScreenType;

    public void Show()
    {
        if(UIManager.Instance != null)
        UIManager.Instance.ShowScreen(ScreenType);
    }
}
