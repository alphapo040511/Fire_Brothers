using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    private const string c_SaveFileName = "stage_save.json";
    private StageSaveWrapper saveData = new StageSaveWrapper();

    [Header("Resources에서 불러올 StageInfo 데이터")]
    private StageInfoWrapper stageInfoData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStageInfo();
            LoadStageSave();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStageInfo()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("DataBase/StageInfo");

        if (jsonFile == null)
        {
            Debug.LogError("StageInfo.json 파일을 Resources/DataBase 경로에 배치해야 합니다.");
            return;
        }

        stageInfoData = JsonUtility.FromJson<StageInfoWrapper>(jsonFile.text);
    }

    private void LoadStageSave()
    {
        string path = Path.Combine(Application.persistentDataPath, c_SaveFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<StageSaveWrapper>(json);
        }
        else
        {
            InitializeNewSaveData();

            if(DataManager.Instance != null)                //스테이지 정보가 없는 경우에는 게임 데이터도 초기화
            {
                DataManager.Instance.ResetGamesData();
            }
        }
    }

    public void InitializeNewSaveData()
    {
        saveData = new StageSaveWrapper();
        saveData.saves = new List<StageSave>();

        foreach (var info in stageInfoData.stages)
        {
            saveData.saves.Add(new StageSave
            {
                stageIndex = info.stageIndex,
                isCleared = false,
                bestStars = 0
            });
        }

        saveData.totalStars = 0;
        SaveStageData();
    }

    private void SaveStageData()
    {
        string path = Path.Combine(Application.persistentDataPath, c_SaveFileName);
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public bool IsStageUnlocked(int stageIndex)
    {
        StageInfo info = stageInfoData.stages.Find(x => x.stageIndex == stageIndex);
        if (info == null)
        {
            Debug.LogError($"스테이지 {stageIndex} 정보가 StageInfo에 존재하지 않습니다.");
            return false;
        }

        return saveData.totalStars >= info.unlockRequiredStars;
    }

    public bool IsStageClaer(int index)
    {
        StageSave save = saveData.saves.Find(x => x.stageIndex == index);
        if (save == null)
        {
            Debug.LogError($"스테이지 {index} 정보가 StageInfo에 존재하지 않습니다.");
            return false;
        }

        return save.isCleared;
    }

    public int GetStageBestStars(int stageIndex)
    {
        StageSave save = saveData.saves.Find(x => x.stageIndex == stageIndex);
        return save != null ? save.bestStars : 0;
    }

    public bool CompleteStage(int stageIndex, int earnedScore)
    {
        StageSave save = saveData.saves.Find(x => x.stageIndex == stageIndex);
        StageInfo info = stageInfoData.stages.Find(x => x.stageIndex == stageIndex);

        if (save == null || info == null)
        {
            Debug.LogError($"스테이지 {stageIndex} 완료 처리 중 데이터 오류 발생");
            return false;
        }

        int earnedStars = CalculateStars(earnedScore, info.starScoreThresholds);

        if(earnedStars == 0)
        {
            Debug.Log("클리어 실패");
            return false;
        }

        if (earnedStars > save.bestStars)
        {
            int additionalStars = earnedStars - save.bestStars;
            save.bestStars = earnedStars;
            saveData.totalStars += additionalStars;
        }

        save.isCleared = true;
        SaveStageData();
        return true;
    }

    public int CalculateStars(int score, int[] thresholds)
    {
        if (score >= thresholds[2]) return 3;
        if (score >= thresholds[1]) return 2;
        if (score >= thresholds[0]) return 1;
        return 0;
    }

    public int GetTotalStars()
    {
        return saveData.totalStars;
    }

    public int[] GetStarThreshold()
    {
        if(stageInfoData != null && stageInfoData.stages.Count > GameManager.Instance.currentStageIndex)
        {
            return stageInfoData.stages[GameManager.Instance.currentStageIndex].starScoreThresholds;
        }

        return new int[3] {1000,1000,1000};
    }
}
