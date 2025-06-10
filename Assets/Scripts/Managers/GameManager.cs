using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Menu,       // 메인 메뉴 상태
    Ready,      // 플레이 대기 중
    Playing,    // 게임 플레이 중
    Paused,     // 일시정지 상태
    GameOver,   // 게임 종료 상태
    Loading,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //현재 게임 상태
    public GameState CurrentState;

    public event Action<GameState> OnGameStateChanged;                      //게임 상태 변경 이벤트

    public int currentStageIndex;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //맵 로딩 및 연출 완료시 시작 되도록 변경
        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Menu:
                Time.timeScale = 1f;
                break;

            case GameState.Ready:
                Time.timeScale = 0f;
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 1f;
                //무언가 게임 오버시 연출들?
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void CurrentStageSetting(int index)
    {
        currentStageIndex = index;
    }
}
