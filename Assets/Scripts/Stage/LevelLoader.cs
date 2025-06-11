using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private const string c_PrefabDatabasePath = "Database/PrefabIndexDatabase";

    void Start()
    {
        if(GameManager.Instance != null)
        StartCoroutine(Loading(GameManager.Instance.currentStageIndex));
    }


    private IEnumerator Loading(int levelIndex)
    {
        // 데이터베이스 로드
        PrefabIndexDatabase db = Resources.Load<PrefabIndexDatabase>(c_PrefabDatabasePath);
        if (db == null)
        {
            Debug.LogError("PrefabIndexDatabase 로드 실패");
            yield break;
        }

        // JSON 로드
        string json;
        string filePath = Path.Combine(Application.persistentDataPath, $"Level_{levelIndex}.json");
        if (File.Exists(filePath))
        {
            json = File.ReadAllText(filePath);
        }
        else
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>($"Json/Level_{levelIndex}");
            if (jsonAsset == null)
            {
                Debug.LogError($"레벨 {levelIndex} JSON 리소스 없음");
                yield break;
            }
            json = jsonAsset.text;
        }

        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        // 프리팹 인스턴스
        foreach (var objData in levelData.objectDatas)
        {
            GameObject prefab = db.GetPrefabByIndex(objData.prefabIndex);
            if (!prefab)
            {
                Debug.LogError($"프리팹 인덱스 {objData.prefabIndex} 로드 실패");
                continue;
            }
            Transform tf = Object.Instantiate(
                prefab,
                objData.position,
                Quaternion.Euler(objData.rotation)
            ).transform;
            tf.localScale = objData.scale;

            yield return null;
        }

        Debug.Log($"Level {levelIndex} 로드 완료 ({levelData.objectDatas.Count})");
        GameManager.Instance.ChangeState(GameState.Ready);
    }
}
