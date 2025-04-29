using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public class PrefabIndexDatabaseGenerator
{
    private const string c_OutputPath = "Assets/Resources/Database";
    private const string c_OutputAssetName = "PrefabIndexDatabase.asset";
    private const string c_ScanFolder = "Assets/AddressablePrefabs";

    [MenuItem("Tools/Generate Prefab Index Database (Addressables)")]
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

            // Addressable 등록
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guids[i], settings.DefaultGroup);

            entry.address = path.Replace("Assets/AddressablePrefabs/", "").Replace(".prefab", "");

            var reference = new AssetReferenceGameObject(entry.guid);

            database.prefabList.Add(new PrefabIndexDatabase.PrefabIndexEntry
            {
                prefabRef = reference,
                prefabIndex = i
            });
        }

        if (!Directory.Exists(c_OutputPath))
            Directory.CreateDirectory(c_OutputPath);

        string fullPath = Path.Combine(c_OutputPath, c_OutputAssetName);
        AssetDatabase.CreateAsset(database, fullPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"PrefabIndexDatabase(Addressable) 생성 완료 ({database.prefabList.Count}개 등록)");
    }
}
