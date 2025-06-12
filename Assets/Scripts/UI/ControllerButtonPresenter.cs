using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ControllerButtonPresenter : MonoBehaviour
{
    public GameObject keyboardImage;
    public GameObject gamepadImage;

    private void OnEnable()
    {
        InputDeviceManager.Instance.OnDevicesChange += ChangeDevice;
    }

    private void OnDisable()
    {
        InputDeviceManager.Instance.OnDevicesChange -= ChangeDevice;
    }

    private void Start()
    {
        ChangeDevice();
    }

    public void ChangeDevice()
    {
        if (InputDeviceManager.Instance.InputDevices.ContainsKey(0))
        {
            bool isKeyboard = InputDeviceManager.Instance.InputDevices[0] is Keyboard keyboard;
            keyboardImage.SetActive(isKeyboard);
            gamepadImage.SetActive(!isKeyboard);
        }
    }
}
