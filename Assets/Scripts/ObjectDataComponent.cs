using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//JSON 변환 대상 오브젝트의 컴포넌트
public class ObjectDataComponent : MonoBehaviour
{
    [SerializeField]
    private int m_PrefabIndex = 0;
    public int PrefabIndex => m_PrefabIndex;
}
