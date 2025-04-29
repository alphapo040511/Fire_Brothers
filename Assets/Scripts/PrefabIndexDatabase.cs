using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "PrefabIndexDatabase", menuName = "Database/PrefabIndexDatabase")]
public class PrefabIndexDatabase : ScriptableObject
{
    [System.Serializable]
    public class PrefabIndexEntry
    {
        public AssetReferenceGameObject prefabRef; // Addressable 참조
        public int prefabIndex;
    }

    public List<PrefabIndexEntry> prefabList = new();

    public AssetReferenceGameObject GetPrefabReferenceByIndex(int index)
    {
        var entry = prefabList.Find(p => p.prefabIndex == index);
        return entry != null ? entry.prefabRef : null;
    }
}

