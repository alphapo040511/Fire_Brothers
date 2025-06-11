using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayableObjectsManager : MonoBehaviour
{
    public static PlayableObjectsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Action<bool> CanMoveChanged;

    public Transform fireTruckPosition;
    public Transform ambulancePosition;

    public VehicleMove fireTruckPref;
    public VehicleMove ambulancePref;

    private VehicleMove fireTruck;

    private List<Transform> vehiclesPos = new List<Transform>();
    private Transform[] playersPosition;

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

    private void Start()
    {
        GameReady();
    }

    private void Update()
    {
        if(ready)
        {
            transform.position = CenterPosition();
        }
    }

    public void AccessibleChecking()
    {
        CanMoveChanged.Invoke(fireTruck.waypoint.isAccessible);
    }

    private void GameReady()
    {
        if (fireTruckPref != null && fireTruckPosition != null)
        {
            fireTruck = Instantiate(fireTruckPref, fireTruckPosition.position, transform.rotation);
            CanMoveChanged += fireTruck.OnCanMoveChanged;
            vehiclesPos.Add(fireTruck.transform);
        }

        if(ambulancePref != null && ambulancePosition != null)
        {
            VehicleMove ambul = Instantiate(ambulancePref, ambulancePosition.position, transform.rotation);
            CanMoveChanged += ambul.OnCanMoveChanged;
            vehiclesPos.Add(ambul.transform);
        }

        ready = true;
    }


    private Vector3 CenterPosition()
    {
        Vector3 sum = Vector3.zero;
        for(int i = 0; i < vehiclesPos.Count; i++)
        {
            sum += vehiclesPos[i].position - vehiclesPos[i].transform.forward;
        }

        return sum / (vehiclesPos.Count > 0 ? vehiclesPos.Count : 1);
    }

    public void ChangeState(GameState newState)
    {

        switch (newState)
        {
            case GameState.Ready:
                CanMoveChanged?.Invoke(false);
                break;

            case GameState.Paused:
            case GameState.GameOver:
                CanMoveChanged?.Invoke(false);
                ready = false;
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
