using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceManager
{
    private static InputDeviceManager instance;
    public static InputDeviceManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new InputDeviceManager();
            }

            return instance;
        }
    }

    public Dictionary<InputDevice, int> inputDevices = new Dictionary<InputDevice, int>();
    public event Action<InputDevice> OnInputDeviceConnected;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    public bool IsConnectedDevice(InputDevice newDevice)
    {
        return inputDevices.ContainsKey(newDevice);
    }

    public void ConnectNewDevice(InputDevice newDevice)
    {
        if(inputDevices.ContainsKey(newDevice) || inputDevices.Count >= 2) return;

        if(inputDevices.ContainsValue(1))
        {
            inputDevices[newDevice] = 2;
        }
        else
        {
            inputDevices[newDevice] = 1;
        }

        OnInputDeviceConnected?.Invoke(newDevice);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (inputDevices.ContainsKey(device) && change == InputDeviceChange.Removed)
        {
            inputDevices.Remove(device);
        }
    }
}
