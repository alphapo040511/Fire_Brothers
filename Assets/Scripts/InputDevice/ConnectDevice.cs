using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectDevice : MonoBehaviour
{
    public TextMeshProUGUI[] deviceText = new TextMeshProUGUI[2];

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
                if (keyboard.enterKey.wasPressedThisFrame)
                {
                    InputDeviceManager.Instance.ConnectNewKeyboard(device);
                }
            }
        }
    }


    //임시로 현재 연결된 디바이스 확인
    private void CheckDevice()
    {
        if (deviceText[0] == null || deviceText[1] == null) return;

        int count = 0;

        deviceText[0].text = "Disconnected";
        deviceText[1].text = "Disconnected";

        foreach (var device in InputDeviceManager.Instance.InputDevices)
        {
            if(device.Key < 2)
            {
                deviceText[device.Key].text = "Connected";
                count++;
            }
        }

        Debug.Log(count + " 개 연결됨");

        if (UIManager.Instance != null)
        {
            if (count >= 2)
            {
                UIManager.Instance.HideScreen();
            }
            else if (UIManager.Instance.CurrentScreen != ScreenType.ControllerSet)
            {
                UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
            }
        }
    }
}
