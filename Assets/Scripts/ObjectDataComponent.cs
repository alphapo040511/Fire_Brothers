using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//JSON 변환 대상 오브젝트의 컴포넌트
public class ObjectDataComponent : MonoBehaviour
{
    [SerializeField] private string m_PrefabName;

    public string prefabName => m_PrefabName;

    private void Reset()
    {
        m_PrefabName = gameObject.name;
    }
}
