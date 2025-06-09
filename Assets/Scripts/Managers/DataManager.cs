using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    //게임 데이터
    public GameData gameData { get; private set; }

    //데이터 파일 경로
    private string saveFilePath;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //저장 경로 설정
        saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

        Initialize();
    }

    private void Initialize()
    {
        if(LoadData())      //기존 데이터가 있는 경우
        {
            Debug.Log("데이터 로드 완료");
        }
        else
        {
            //로드 실패 시
            gameData = new GameData();

            //게임 데이터는 스테이지 클리어 정보에 영향을 끼치진 않으니 게임 데이터만 초기화
        }
    }

    public void SetVolume(AudioType type, float value)
    {
        switch (type)
        {
            case AudioType.Master:
                gameData.masterVolume = value; 
                break;
            case AudioType.BGM:
                gameData.bgmVolume = value;
                break;
            case AudioType.SFX:
                gameData.sfxVolume = value;
                break;
        }
    }

    public bool SaveData()
    {
        try
        {
            // 데이터 직렬화 (JSON)
            string jsonData = JsonUtility.ToJson(gameData, true);

            // 파일에 저장
            File.WriteAllText(saveFilePath, jsonData);
            Debug.Log("데이터 저장 완료");

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
            return false;
        }
    }

    private bool LoadData()
    {
        try
        {
            if(File.Exists(saveFilePath))
            {
                string jsonData = File.ReadAllText(saveFilePath);

                gameData = JsonUtility.FromJson<GameData>(jsonData);

                return true;
            }
            else
            {
                Debug.LogWarning("세이브 데이터가 존재하지 않습니다.");
                return false;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError("Failed to save game data: " + e.Message);
            return false;
        }
    }

    //게임 데이터 초기화
    public void ResetGamesData()
    {
        // 게임 설정
        float masterVolume = gameData.masterVolume;
        float bgmVolume = gameData.bgmVolume;
        float sfxVolume = gameData.sfxVolume;
        bool isMuted = gameData.isMuted;

        gameData = new GameData();

        gameData.masterVolume = masterVolume;
        gameData.bgmVolume = bgmVolume;
        gameData.sfxVolume = sfxVolume;
        gameData.isMuted = isMuted;

        if(StageManager.Instance)
        {
            StageManager.Instance.InitializeNewSaveData();          //세이브 데이터가 쪼개져 있어서 따로 진행
        }

        SaveData();     //데이터 저장
    }

    public void UpdateCustomizeData(int playerIndex , ClothesType type, int clothesIndex)
    {
        if (gameData.playersCustomData[playerIndex].customizeData[(int)type].type == type)
        {
            gameData.playersCustomData[playerIndex].customizeData[(int)type].index = clothesIndex;
        }
    }

}
