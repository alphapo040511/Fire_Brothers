using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabIndexDatabaseGenerator
{
    private const string c_OutputPath = "Assets/Resources/Database";
    private const string c_OutputAssetName = "PrefabIndexDatabase.asset";
    private const string c_ScanFolder = "Assets/Resources/Prefabs";

    [MenuItem("Tools/Generate Prefab Index Database (Resources)")]
    public static void GenerateDatabase()
    {
        if (!Directory.Exists(c_ScanFolder))
        {
            Debug.LogError($"프리팹 폴더가 존재하지 않습니다: {c_ScanFolder}");
            return;
        }

        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { c_ScanFolder });
        var database = ScriptableObject.CreateInstance<PrefabIndexDatabase>();

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            string prefabName = Path.GetFileNameWithoutExtension(path);

            bool updated = false;

            // ObjectDataComponent 없으면 자동 추가

            //if (prefab.GetComponent<ObjectDataComponent>() == null)
            //{
            //    var tempInstance = Object.Instantiate(prefab);
            //    tempInstance.AddComponent<ObjectDataComponent>();
            //    PrefabUtility.SaveAsPrefabAsset(tempInstance, path);
            //    Object.DestroyImmediate(tempInstance);
            //    updated = true;
            //}

            //ObjectDataComponent 없으면 건너뛰기 (프리팹 추가 할 때 마다 인덱스가 꼬여서 번호 변경이 없도록 수정)

            // 인덱스 주입
            GameObject updatedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            ObjectDataComponent comp = updatedPrefab.GetComponent<ObjectDataComponent>();
            if (comp != null)
            {
                SerializedObject so = new SerializedObject(comp);
                so.FindProperty("m_PrefabIndex").intValue = comp.PrefabIndex;                   //적어놓은 인덱스로 저장 되도록 수정
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(updatedPrefab);
                updated = true;
            }

            if (updated)
                Debug.Log($"프리팹 수정됨: {prefabName}");

            // DB에 등록
            database.prefabList.Add(new PrefabIndexDatabase.PrefabIndexEntry
            {
                prefabName = prefabName,
                prefabIndex = i
            });
        }

        if (!Directory.Exists(c_OutputPath))
            Directory.CreateDirectory(c_OutputPath);

        string fullPath = Path.Combine(c_OutputPath, c_OutputAssetName);
        AssetDatabase.CreateAsset(database, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"PrefabIndexDatabase(Resources) 생성 완료 ({database.prefabList.Count}개 등록)");
    }
}
