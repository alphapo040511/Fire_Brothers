#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class StageInfoEditor : EditorWindow
{
    private List<StageInfo> stageInfos = new List<StageInfo>();
    private Vector2 scrollPos;
    private string savePath = "Assets/Resources/DataBase/StageInfo.json";

    [MenuItem("Tools/Stage Info Editor")]
    public static void OpenWindow()
    {
        StageInfoEditor window = GetWindow<StageInfoEditor>("Stage Info Editor");
        window.LoadStageInfoJson();
    }

    private void OnGUI()
    {
        GUILayout.Label("스테이지 데이터 편집", EditorStyles.boldLabel);

        if (GUILayout.Button("새 스테이지 추가"))
        {
            stageInfos.Add(new StageInfo
            {
                stageIndex = stageInfos.Count,
                starScoreThresholds = new int[3] { 100, 300, 500 },
                unlockRequiredStars = 0
            });
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < stageInfos.Count; i++)
        {
            var info = stageInfos[i];

            GUILayout.BeginVertical("box");
            info.stageIndex = EditorGUILayout.IntField("Stage Index", info.stageIndex);
            info.starScoreThresholds[0] = EditorGUILayout.IntField("별 1개 점수", info.starScoreThresholds[0]);
            info.starScoreThresholds[1] = EditorGUILayout.IntField("별 2개 점수", info.starScoreThresholds[1]);
            info.starScoreThresholds[2] = EditorGUILayout.IntField("별 3개 점수", info.starScoreThresholds[2]);
            info.unlockRequiredStars = EditorGUILayout.IntField("해금에 필요한 총 별 수", info.unlockRequiredStars);

            if (GUILayout.Button("이 스테이지 삭제"))
            {
                stageInfos.RemoveAt(i);
                break;
            }
            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        if (GUILayout.Button("StageInfo.json 저장"))
        {
            SaveStageInfoJson();
        }

        if (GUILayout.Button("StageInfo.json 다시 불러오기"))
        {
            LoadStageInfoJson();
        }
    }

    private void SaveStageInfoJson()
    {
        var wrapper = new StageInfoWrapper { stages = stageInfos };
        string json = JsonUtility.ToJson(wrapper, true);

        if (!Directory.Exists("Assets/Resources/DataBase"))
        {
            Directory.CreateDirectory("Assets/Resources/DataBase");
        }

        File.WriteAllText(savePath, json);
        AssetDatabase.Refresh();
        Debug.Log("StageInfo.json 저장 완료");
    }

    private void LoadStageInfoJson()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("StageInfo.json 파일이 존재하지 않습니다. 새로운 파일을 생성합니다.");
            stageInfos = new List<StageInfo>();
            return;
        }

        string json = File.ReadAllText(savePath);
        StageInfoWrapper wrapper = JsonUtility.FromJson<StageInfoWrapper>(json);

        if (wrapper != null && wrapper.stages != null)
        {
            stageInfos = wrapper.stages;
            Debug.Log($"StageInfo.json 로드 완료. {stageInfos.Count}개 스테이지 불러옴");
        }
        else
        {
            Debug.LogWarning("StageInfo.json 파일 내용이 올바르지 않습니다. 초기화합니다.");
            stageInfos = new List<StageInfo>();
        }
    }
}
#endif
