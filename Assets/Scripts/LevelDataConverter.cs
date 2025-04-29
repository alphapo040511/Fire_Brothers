using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
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
    private const string c_PrefabIndexDatabasePath = "Database/PrefabIndexDatabase"; // Resources 기준


    public static void SaveLevelData(int levelIndex)
    {
        // 저장할 데이터 구성
        LevelData data = new LevelData
        {
            levelIndex = levelIndex,
            objectDatas = new List<ObjectData>()
        };

        ObjectDataComponent[] objects = GameObject.FindObjectsOfType<ObjectDataComponent>();
        for (int i = 0; i < objects.Length; i++)
        {
            var tf = objects[i].transform;
            data.objectDatas.Add(new ObjectData
            {
                index = i,
                prefabIndex = objects[i].PrefabIndex,
                position = tf.position,
                rotation = tf.rotation.eulerAngles,
                scale = tf.localScale
            });
        }

        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine("Assets/Resources/Json", $"Level_{levelIndex}.json");
        File.WriteAllText(filePath, json);
        Debug.Log($"[LevelData] {filePath} 저장 완료 ({data.objectDatas.Count}개 오브젝트)");
    }
    public static void LoadLevelDataAddressable(MonoBehaviour runner, int levelIndex)
    {
        runner.StartCoroutine(LoadCoroutine(levelIndex));
    }

    private static IEnumerator LoadCoroutine(int levelIndex)
    {
        string path = Path.Combine(c_SavePath, $"Level_{levelIndex}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"레벨 {levelIndex} JSON 파일 없음");
            yield break;
        }

        PrefabIndexDatabase database = Resources.Load<PrefabIndexDatabase>(c_PrefabIndexDatabasePath);
        if (database == null)
        {
            Debug.LogError("PrefabIndexDatabase.asset이 Resources/Database에 존재하지 않음");
            yield break;
        }

        string json = File.ReadAllText(path);
        LevelData data = JsonUtility.FromJson<LevelData>(json);

        foreach (ObjectData objData in data.objectDatas)
        {
            var prefabRef = database.GetPrefabReferenceByIndex(objData.prefabIndex);
            if (prefabRef == null)
            {
                Debug.LogError($"프리팹 인덱스 {objData.prefabIndex}에 해당하는 Addressable 참조 없음");
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
                Debug.LogError($"프리팹 인스턴스화 실패: 인덱스 {objData.prefabIndex}");
            }
        }

        Debug.Log($"레벨 {levelIndex} Addressables 기반 로드 완료: {data.objectDatas.Count}개 생성됨");
    }
}