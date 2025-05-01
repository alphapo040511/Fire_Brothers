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
            if (instance == null)
            {
                instance = new InputDeviceManager();
                InputSystem.onDeviceChange += OnDeviceChange;
            }

            return instance;
        }
    }

    private Dictionary<InputDevice, int> inputDevices = new Dictionary<InputDevice, int>();
    public IReadOnlyDictionary<InputDevice, int> InputDevices => inputDevices; // 외부에서는 읽기 전용으로 공개
    public event Action<InputDevice> OnInputDeviceConnected;

    public bool IsConnectedDevice(InputDevice newDevice)
    {
        return inputDevices.ContainsKey(newDevice);
    }

    public void ConnectNewDevice(InputDevice newDevice)
    {
        if (inputDevices.ContainsKey(newDevice) || inputDevices.Count >= 2) return;

        if (inputDevices.ContainsValue(0))
        {
            inputDevices.Add(newDevice, 1);
        }
        else
        {
            inputDevices.Add(newDevice, 0);
        }

        Debug.Log($"{newDevice}가 연결됨");

        OnInputDeviceConnected?.Invoke(newDevice);
    }

    public InputDevice FindDevice(int index)
    {
        foreach(var input in InputDevices)
        {
            if(input.Value == index)
            {
                return input.Key;
            }
        }

        return null;
    }

    private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (Instance.inputDevices.ContainsKey(device) && change == InputDeviceChange.Removed)
        {
            Instance.inputDevices.Remove(device);
        }
    }
}
