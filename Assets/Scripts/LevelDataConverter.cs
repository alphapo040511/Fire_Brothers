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
        string path = Path.Combine(c_SavePath, $"Level_{levelIndex}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"레벨 {levelIndex} JSON 파일 없음");
            return;
        }

        PrefabIndexDatabase db = Resources.Load<PrefabIndexDatabase>(c_PrefabDatabasePath);
        if (db == null)
        {
            Debug.LogError("PrefabIndexDatabase를 Resources/Database에 배치해야 합니다.");
            return;
        }

        string json = File.ReadAllText(path);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        foreach (var objData in levelData.objectDatas)
        {
            string prefabName = db.GetPrefabNameByIndex(objData.prefabIndex);
            GameObject prefab = Resources.Load<GameObject>($"{c_PrefabPath}{prefabName}");
            if (prefab == null)
            {
                Debug.LogError($"프리팹 로드 실패: {prefabName}");
                continue;
            }

            GameObject instance = Object.Instantiate(prefab, objData.position, Quaternion.Euler(objData.rotation));
            instance.transform.localScale = objData.scale;
        }

        Debug.Log($"Level {levelIndex} 로드 완료");
    }
}