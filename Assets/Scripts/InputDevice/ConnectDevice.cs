using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConnectDevice : MonoBehaviour
{
    public Image[] deviceImage = new Image[2];
    public Sprite controllerImage;
    public Sprite keyboardImage;

    private void Start()
    {
        CheckDevice();
        InputDeviceManager.Instance.AddEvent();
        InputDeviceManager.Instance.OnDevicesChange += CheckDevice;
    }

    private void OnEnable()
    {
        CheckDevice();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.devices.Count == 0) return;

        foreach (InputDevice device in InputSystem.devices)
        {
            if (device is Gamepad gamepad)
            {
                if (gamepad.leftShoulder.isPressed && gamepad.rightShoulder.isPressed)
                {
                    InputDeviceManager.Instance.ConnectNewDevice(device);
                }
            }
            else if (device is Keyboard keyboard)
            {
                if (keyboard.spaceKey.wasPressedThisFrame)
                {
                    InputDeviceManager.Instance.ConnectNewKeyboard(device);
                }
            }
        }
    }


    //임시로 현재 연결된 디바이스 확인
    private void CheckDevice()
    {
        if (deviceImage[0] == null || deviceImage[1] == null) return;

        int count = 0;

        deviceImage[0].sprite = null;
        deviceImage[1].sprite = null;

        foreach (var device in InputDeviceManager.Instance.InputDevices)
        {
            if(device.Key < 2)
            {
                if(device.Value is Keyboard keyboard)
                {
                    deviceImage[device.Key].sprite = keyboardImage;
                }
                else if(device.Value is Gamepad gamepad)
                {
                    deviceImage[device.Key].sprite = controllerImage;
                }

                count++;
            }
        }

        Debug.Log(count + " 개 연결됨");

        if (UIManager.Instance != null)
        {
            if (count >= 2)
            {
                UIManager.Instance.HideScreen(0.5f);
            }
            else if (UIManager.Instance.CurrentScreen != ScreenType.ControllerSet)
            {
                UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
            }
        }
    }
}
