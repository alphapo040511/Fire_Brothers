#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 레벨 데이터 저장과 Addressables 기반 불러오기를 지원하는 툴
public class LevelDataToolEditor : EditorWindow
{
    private int m_LevelIndex = 1;

    [MenuItem("Tools/Level Data Tool")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataToolEditor>("Level Data Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Data Save / Load / Clear", EditorStyles.boldLabel);

        m_LevelIndex = EditorGUILayout.IntField("Level Index", m_LevelIndex);

        if (GUILayout.Button("Save Level Data"))
        {
            LevelDataConverter.SaveLevelData(m_LevelIndex);
        }

        if (GUILayout.Button("Load Level Data (Addressables)"))
        {
            EditorCoroutineUtility.StartCoroutine(LoadCoroutineInEditor(m_LevelIndex), this);
        }

        GUILayout.Space(10);

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Scene Level Data"))
        {
            if (EditorUtility.DisplayDialog("확인", "씬의 모든 ObjectDataComponent 오브젝트를 삭제하시겠습니까?", "삭제", "취소"))
            {
                ClearSceneObjects();
            }
        }
        GUI.backgroundColor = Color.white;
    }

    private void ClearSceneObjects()
    {
        ObjectDataComponent[] objects = GameObject.FindObjectsOfType<ObjectDataComponent>();
        int count = 0;

        foreach (var obj in objects)
        {
            if (obj != null)
            {
                DestroyImmediate(obj.gameObject);
                count++;
            }
        }

        Debug.Log($"총 {count}개의 ObjectDataComponent 오브젝트가 삭제되었습니다.");
    }

    // Addressables 기반 레벨 데이터 로드용 코루틴
    private static IEnumerator LoadCoroutineInEditor(int levelIndex)
    {
        string path = Path.Combine("Assets/Resources/Json", $"Level_{levelIndex}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"레벨 {levelIndex} JSON 파일이 존재하지 않습니다.");
            yield break;
        }

        PrefabIndexDatabase db = Resources.Load<PrefabIndexDatabase>("Database/PrefabIndexDatabase");
        if (db == null)
        {
            Debug.LogError("PrefabIndexDatabase.asset이 Resources/Database 경로에 없습니다.");
            yield break;
        }

        string json = File.ReadAllText(path);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        foreach (var objData in levelData.objectDatas)
        {
            var prefabRef = db.GetPrefabReferenceByIndex(objData.prefabIndex);
            if (prefabRef == null)
            {
                Debug.LogError($"프리팹 인덱스 {objData.prefabIndex}를 찾을 수 없습니다.");
                continue;
            }

            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefabRef, objData.position, Quaternion.Euler(objData.rotation));
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject instance = handle.Result;
                instance.transform.localScale = objData.scale;
            }
            else
            {
                Debug.LogError($"프리팹 로드 실패: 인덱스 {objData.prefabIndex}");
            }
        }

        Debug.Log($"[Editor] Addressables 기반 Level {levelIndex} 로드 완료");
    }
}
#endif
