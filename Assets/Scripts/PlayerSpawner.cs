using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void OnEnable()
    {
        //InputDeviceManager.Instance.OnInputDeviceConnected += SpawnPlayer;
    }

    private void OnDisable()
    {
        //InputDeviceManager.Instance.OnInputDeviceConnected -= SpawnPlayer;
    }

    public void SpawnPlayer()
    {
        foreach(var value in InputDeviceManager.Instance.InputDevices)
        {
            var player = PlayerInput.Instantiate(playerPrefab, value.Key, controlScheme: null, pairWithDevice: value.Value);
        }
    }

    public void SpawnPlayer(InputDevice device)
    {
        var player = PlayerInput.Instantiate(playerPrefab, InputDeviceManager.Instance.FindIndex(device), controlScheme: null, pairWithDevice: device);
        player.GetComponent<PlayerMovement>().inputDevice = device;
        player.GetComponent<PlayerMovement>().playerIndex = InputDeviceManager.Instance.FindIndex(device);
    }

    public Vector3 GetPosition()
    {
        Vector2 randomPoint = Random.insideUnitCircle * 3;
        return transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
}
