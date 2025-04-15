using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }       //싱글턴 선언

    private const string c_FileName = "stage_data.json";        //저장될 JSON 파일 이름
    private const int c_StageCount = 20;                    //만약 JSON이 비어있다면 스테이지 20까지 임의로 생성

    [Header("초기 스테이지 JSON")]
    [SerializeField] private TextAsset initialStageJson; // 초기 스테이지 설정들 데이터 파일 설정해두면 처음 게임 시작할 때 우리가 제작한 스테이지 데이터로 불러옴

    [SerializeField] private int starsToUnlock = 1;     //스테이지들은 이 전 스테이지에서 별 1개 이상 획득해야 오픈

    public List<StageData> stages = new();              //리스트 선언

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadOrInitalize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string GetPath() => Path.Combine(Application.persistentDataPath, c_FileName);

    private void LoadOrInitalize()
    {
        string path = GetPath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            stages = JsonUtility.FromJson<StageDataWrapper>(json).stages;
        }
        else
        {
            if (initialStageJson != null)
            {
                // 초기 JSON 복사하여 저장
                File.WriteAllText(path, initialStageJson.text);
                Debug.Log("초기 스테이지 데이터 복제 완료");
                stages = JsonUtility.FromJson<StageDataWrapper>(initialStageJson.text).stages;
            }
            else
            {
                Debug.LogWarning("초기 JSON이 설정되지 않아 기본 초기화 진행");
                InitNewData(); // 예외 fallback
            }
        }
    }

    private void InitNewData()
    {
        stages.Clear();
        for (int i = 0; i < c_StageCount; i++)
        {
            stages.Add(new StageData
            {
                stageIndex = i,
                isCleared = false,
                stars = 0,
                isUnlockedWithoutStars = (i == 0)
            });
        }
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(new StageDataWrapper { stages = stages }, true);
        File.WriteAllText(GetPath(), json);
    }

    public bool IsStageUnlocked(int index)
    {
        StageData current = stages.Find(s => s.stageIndex == index);
        if (current == null)
        {
            Debug.LogWarning($"[IsStageUnlocked] 스테이지 {index} 정보 없음");
            return false;
        }

        if (current.isUnlockedWithoutStars)
            return true;

        StageData prev = stages.Find(s => s.stageIndex == index - 1);
        if (prev == null)
        {
            Debug.LogWarning($"[IsStageUnlocked] 이전 스테이지 {index - 1} 정보 없음");
            return false;
        }

        return prev.isCleared && prev.stars >= starsToUnlock;
    }

    public void CompleteStage(int index, int earnedStars)
    {
        if (index < 0 || index >= stages.Count) return;
        
        var data = stages[index];
        data.isCleared = true;
        data.stars = Mathf.Max(data.stars, earnedStars);
        Save();
    }

    public void UnlockWithoutStars(int index)
    {
        if (index < 0 || index >= stages.Count) return;
        stages[index].isUnlockedWithoutStars = true;
        Save();
    }

    public int GetStars(int index)
    {
        if (index < 0 || index >= stages.Count) return 0;
        return stages[index].stars;
    }
}
