using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void OnEnable()
    {
        InputDeviceManager.Instance.OnInputDeviceConnected += SpawnPlayer;
    }

    private void OnDisable()
    {
        InputDeviceManager.Instance.OnInputDeviceConnected -= SpawnPlayer;
    }

    public void SpawnPlayer()
    {
        foreach(var value in InputDeviceManager.Instance.inputDevices)
        {
            var player = PlayerInput.Instantiate(playerPrefab, value.Value, controlScheme: "PC", pairWithDevice: value.Key);
        }
    }

    public void SpawnPlayer(InputDevice device)
    {
        var player = PlayerInput.Instantiate(playerPrefab, InputDeviceManager.Instance.inputDevices[device], controlScheme: "PC", pairWithDevice: device);
    }

    public Vector3 GetPosition()
    {
        Vector2 randomPoint = Random.insideUnitCircle * 3;
        return transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
}
