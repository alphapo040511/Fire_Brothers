using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConnectDevice : MonoBehaviour
{
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
}
