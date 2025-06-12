using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTitleUI : MonoBehaviour
{
    public void Confirm()
    {
        SceneLoader.LoadScene("TitleScene");
    }

    public void Cancle()
    {
        UIManager.Instance.HideScreen();
    }
}
