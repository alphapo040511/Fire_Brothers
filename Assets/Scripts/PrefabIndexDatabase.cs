using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabIndexDatabase", menuName = "Database/Prefab Index Database")]
public class PrefabIndexDatabase : ScriptableObject
{
    [System.Serializable]
    public class PrefabIndexEntry
    {
        public string prefabName;
        public int prefabIndex;
        public GameObject prefab;
    }

    public List<PrefabIndexEntry> prefabList = new();

    public string GetPrefabNameByIndex(int index)
    {
        var entry = prefabList.Find(e => e.prefabIndex == index);
        return entry != null ? entry.prefabName : string.Empty;
    }

    public GameObject GetPrefabByIndex(int index)
    {
        var entry = prefabList.Find(e => e.prefabIndex == index);
        return entry != null ? entry.prefab : null;
    }
}
