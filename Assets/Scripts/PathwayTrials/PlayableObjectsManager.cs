using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableObjectsManager : MonoBehaviour
{
    public Action<bool> CanMoveChanged;

    public GameObject firstPlayerPref;
    public GameObject secondPlayerPref;

    public VehicleMove fireTruck;
    public VehicleMove ambulance;

    private List<Transform> vehiclesPos = new List<Transform>();

    private bool ready = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += ChangeState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeState;
    }

    private void Update()
    {
        if(ready)
        {
            transform.position = CenterPosition();
        }
    }

    private void GameReady()
    {
        if (fireTruck != null)
        {
            CanMoveChanged += fireTruck.OnCanMoveChanged;
            vehiclesPos.Add(fireTruck.transform);
        }

        if(ambulance != null)
        {
            CanMoveChanged += ambulance.OnCanMoveChanged;
            vehiclesPos.Add(ambulance.transform);
        }


        if (InputDeviceManager.Instance != null)
        {
            foreach (var value in InputDeviceManager.Instance.InputDevices)
            {
                //SpawnPlayer(value.Key);
            }
        }

        GameManager.Instance.ChangeState(GameState.Playing);        //나중에는 시작 버튼 만드는걸로
    }

    public void SpawnPlayer(InputDevice device)
    {
        var player = PlayerInput.Instantiate(firstPlayerPref, InputDeviceManager.Instance.InputDevices[device], controlScheme: null, pairWithDevice: device);
        player.GetComponent<PlayerMovement>().inputDevice = device;
        player.GetComponent<PlayerMovement>().playerIndex = InputDeviceManager.Instance.InputDevices[device];
        player.transform.position = GetPosition();
    }


    private Vector3 CenterPosition()
    {
        Vector3 sum = Vector3.zero;
        for(int i = 0; i < vehiclesPos.Count; i++)
        {
            sum += vehiclesPos[i].position;
        }

        return sum / (vehiclesPos.Count > 0 ? vehiclesPos.Count : 1);
    }

    public Vector3 GetPosition()
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * 3;
        return transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    public void ChangeState(GameState newState)
    {

        switch (newState)
        {
            case GameState.Ready:
                GameReady();
                CanMoveChanged?.Invoke(false);
                break;

            case GameState.Paused:
            case GameState.GameOver:
                CanMoveChanged?.Invoke(false);
                break;

            case GameState.Playing:
                CanMoveChanged?.Invoke(true);
                ready = true;
                break;

            default:
                CanMoveChanged?.Invoke(false);
                break;
        }
    }
}
