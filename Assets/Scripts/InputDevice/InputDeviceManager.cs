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
            }

            return instance;
        }
    }

    

    private Dictionary<int, InputDevice> inputDevices = new Dictionary<int, InputDevice>();
    public IReadOnlyDictionary<int, InputDevice> InputDevices => inputDevices; // 외부에서는 읽기 전용으로 공개
    public event Action OnDevicesChange;

    public void AddEvent()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    public bool IsConnectedDevice(InputDevice newDevice)
    {
        return inputDevices.ContainsValue(newDevice);
    }

    public void ConnectNewDevice(InputDevice newDevice)
    {
        if (inputDevices.ContainsValue(newDevice) || inputDevices.Count >= 2) return;

        if (inputDevices.ContainsKey(0))
        {
            inputDevices.Add(1, newDevice);
        }
        else
        {
            inputDevices.Add(0, newDevice);
        }

        Debug.Log($"{newDevice}가 연결됨");

        OnDevicesChange?.Invoke();
    }

    public void ConnectNewKeyboard(InputDevice newDevice)
    {
        if (inputDevices.Count >= 2) return;

        inputDevices.Add(inputDevices.Count, newDevice);

        Debug.Log($"{newDevice}가 {inputDevices.Count -1}번으로 연결됨");

        OnDevicesChange?.Invoke();
    }

    public int FindIndex(InputDevice device)
    {
        foreach(var input in InputDevices)
        {
            if(input.Value == device)
            {
                return input.Key;
            }
        }

        return -1;
    }

    public void ResetDevices()
    {
        inputDevices = new Dictionary<int, InputDevice>();
    }


    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (Instance.inputDevices.ContainsValue(device) && change == InputDeviceChange.Removed)
        {
            Instance.inputDevices.Remove(FindIndex(device));
            OnDevicesChange?.Invoke();
            if(UIManager.Instance != null)
            {
                UIManager.Instance.ShowScreen(ScreenType.ControllerSet);
            }
        }
    }
}
