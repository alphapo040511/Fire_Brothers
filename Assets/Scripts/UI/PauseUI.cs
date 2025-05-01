using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public void ShowControllerSetting()
    {
        UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
    }

    public void Quit()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if(index == 0)
        {
            Application.Quit();
        }
        else if(index == 1)
        {
            SceneManager.LoadScene(0);
        }
        else if(index >= 2)
        {
            SceneManager.LoadScene(1);
        }
    }
}
