using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelData
{
    public int levelIndex;
    public List<ObjectData> objectDatas = new List<ObjectData>();
}

[System.Serializable]
public class ObjectData
{
    public int index;
    public int prefabIndex;   //프리팹 이름 리소스 폴더 안에 프리팹 이름과 같아야 함
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}

//레벨 데이터를 JSON으로 저장하고 불러오는 기능 구현
public static class LevelDataConverter
{
    private const string c_SavePath = "Assets/Resources/Json";
    private const string c_PrefabPath = "Prefabs/";
    private const string c_PrefabDatabasePath = "Database/PrefabIndexDatabase";

    public static void SaveLevelData(int levelIndex)
    {
        LevelData levelData = new LevelData { levelIndex = levelIndex, objectDatas = new List<ObjectData>() };

        ObjectDataComponent[] objects = Object.FindObjectsOfType<ObjectDataComponent>();
        for (int i = 0; i < objects.Length; i++)
        {
            Transform tf = objects[i].transform;
            levelData.objectDatas.Add(new ObjectData
            {
                index = i,
                prefabIndex = objects[i].PrefabIndex,
                position = tf.position,
                rotation = tf.rotation.eulerAngles,
                scale = tf.localScale
            });
        }

        string json = JsonUtility.ToJson(levelData, true);
        string path = Path.Combine(c_SavePath, $"Level_{levelIndex}.json");
        File.WriteAllText(path, json);
        Debug.Log($"Level {levelIndex} 저장 완료 ({levelData.objectDatas.Count}개 오브젝트)");
    }

    public static void LoadLevelData(int levelIndex)
    {
        // 데이터베이스 로드
        PrefabIndexDatabase db = Resources.Load<PrefabIndexDatabase>(c_PrefabDatabasePath);
        if (db == null)
        {
            Debug.LogError("PrefabIndexDatabase 로드 실패");
            return;
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
                return;
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
        }

        Debug.Log($"Level {levelIndex} 로드 완료 ({levelData.objectDatas.Count})");
    }
}
