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
        InputDeviceManager.Instance.OnInputDeviceConnected += InputDeviceConnected;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.devices.Count == 0) return;

        foreach (InputDevice device in InputSystem.devices)
        {
            if (InputDeviceManager.Instance.IsConnectedDevice(device) == false)
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
                    if (keyboard.enterKey.isPressed)
                    {
                        InputDeviceManager.Instance.ConnectNewDevice(device);
                    }
                }
            }
        }
    }

    //비활성화 상태에서도 사용 가능하도록 변경
    public void InputDeviceConnected(InputDevice device)
    {
        CheckDevice();
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
            if(device.Value < 2)
            {
                deviceText[device.Value].text = "Connected";
                count++;
            }
        }

        Debug.Log(count + " 개 연결됨");

        if(count >= 2)
        {
            UIManager.Instance.HideScreen();
        }
        else if(UIManager.Instance.CurrentScreen != ScreenType.ControllerSet)
        {
            UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
        }
    }
}
