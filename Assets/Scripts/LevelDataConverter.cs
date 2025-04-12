using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine;

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
    private const string c_PrefabIndexDatabasePath = "Database/PrefabIndexDatabase"; // Resources 폴더 기준

    //현재 씬의 오브젝트 데이터를 저장
    public static void SaveLevelData(int levelIndex)
    {
        LevelData levelData = new LevelData { levelIndex = levelIndex, objectDatas = new List<ObjectData>() };

        ObjectDataComponent[] objects = Object.FindObjectsOfType<ObjectDataComponent>();
        for (int i = 0; i < objects.Length; i++)
        {
            var tf = objects[i].transform;

            ObjectData data = new ObjectData
            {
                index = i,
                prefabIndex = objects[i].PrefabIndex,
                position = tf.position,
                rotation = tf.rotation.eulerAngles,
                scale = tf.localScale
            };

            levelData.objectDatas.Add(data);
        }

        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Path.Combine(c_SavePath, $"Level_{levelIndex}.json"), json);
    }

    //레벨 데이터를 불러와서 오브젝트 생성
    public static void LoadLevelData(int levelIndex)
    {
        string path = Path.Combine(c_SavePath, $"Level_{levelIndex}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"레벨 {levelIndex} JSON 파일 없음");
            return;
        }

        //데이터 베이스 불러오기
        PrefabIndexDatabase database = Resources.Load<PrefabIndexDatabase>(c_PrefabIndexDatabasePath);
        if (database == null)
        {
            Debug.LogError("PrefabIndexDatabase.asset 파일이 Resources/Database에 없음");
            return;
        }

        string json = File.ReadAllText(path);
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        foreach (ObjectData objData in data.objectDatas)
        {
            string prefabName = database.GetPrefabNameByIndex(objData.prefabIndex);
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError($"인덱스 {objData.prefabIndex}에 해당하는 프리팹 이름 없음");
                continue;
            }

            GameObject prefab = Resources.Load<GameObject>($"{c_PrefabPath}{prefabName}");
            if (prefab == null)
            {
                Debug.LogError($"프리팹 로드 실패: {prefabName}");
                continue;
            }

            GameObject instance = Object.Instantiate(prefab, objData.position, Quaternion.Euler(objData.rotation));
            instance.transform.localScale = objData.scale;
        }
        Debug.Log($"레벨 {levelIndex} 로드 완료: {data.objectDatas.Count}개 오브젝트 생성");
    }
}