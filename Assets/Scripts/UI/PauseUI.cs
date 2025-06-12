using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public Image[] deviceImage;
    public Sprite keyboardImage;
    public Sprite controllerImage;

    private void OnEnable()
    {
        if (InputDeviceManager.Instance == null) return;

        foreach (var device in InputDeviceManager.Instance.InputDevices)
        {
            if (device.Key < 2)
            {
                if (device.Value is Keyboard keyboard)
                {
                    deviceImage[device.Key].sprite = keyboardImage;
                }
                else if (device.Value is Gamepad gamepad)
                {
                    deviceImage[device.Key].sprite = controllerImage;
                }
                else
                {
                    deviceImage[device.Key].sprite = null;
                }
            }
        }
    }

    public void ShowControllerSetting()
    {
        UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
    }

    public void Quit()
    {
        UIManager.Instance.HideScreen();

        DataManager.Instance.SaveData();

        string name = SceneManager.GetActiveScene().name;
        if(name == "TitleScene")
        {
            Application.Quit();
        }
        else if(name == "StageSelectScene")
        {
            SceneManager.LoadScene("TitleScene");
        }
        else if(name == "Chapter1")
        {
            SceneManager.LoadScene("StageSelectScene");
        }
        else if (name == "CustomizeScene")
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
