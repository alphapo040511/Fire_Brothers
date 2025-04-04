using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//프리펩 인덱스 데이터베이스
[CreateAssetMenu(fileName = "PrefabIndexDatabase", menuName = "Tools/Prefab Index Database")]
public class PrefabIndexDatabase : ScriptableObject
{
    [System.Serializable]
    public class PrefabIndexEntry
    {
        public string prefabName;
        public int prefabIndex;
    }

    public List<PrefabIndexEntry> prefabList = new ();

    public string GetPrefabNameByIndex(int index)
    {
        var entry = prefabList.Find(e => e.prefabIndex == index);
        return entry != null ? entry.prefabName : null;
    }
    
}
