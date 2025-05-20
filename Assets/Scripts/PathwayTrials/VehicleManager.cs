using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public Action<bool> CanMoveChanged;
    public VehicleMove fireTruck;
    public VehicleMove ambulance;

    // Start is called before the first frame update
    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += ChangeState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= ChangeState;
    }

    private void GameReady()
    {
        VehicleMove truck = Instantiate(fireTruck, transform.right * 7.5f, transform.rotation);
        CanMoveChanged += truck.OnCanMoveChanged;

        VehicleMove _ambulance = Instantiate(ambulance, transform.right * 7.5f, transform.rotation);
        CanMoveChanged += _ambulance.OnCanMoveChanged;
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
                break;

            default:
                CanMoveChanged?.Invoke(false);
                break;
        }
    }
}
