#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//레벨 데이터 저장과 불러오기 하는 툴이와용
public class LevelDataToolEditor : EditorWindow
{
    private int m_LevelIndex = 1;

    [MenuItem("Tools/LevelDataTool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelDataToolEditor>("Level Data Tool");

    }

    private void OnGUI()
    {
        GUILayout.Label("Level Data Save & Load & Clear", EditorStyles.boldLabel);

        m_LevelIndex = EditorGUILayout.IntField("Level Index", m_LevelIndex);



        if (GUILayout.Button("Save"))
        {
            LevelDataConverter.SaveLevelData(m_LevelIndex);
        }

        if (GUILayout.Button("Load"))
        {
            LevelDataConverter.LoadLevelData(m_LevelIndex);
        }

        GUILayout.Space(10);

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Level Data"))
        {
            if (EditorUtility.DisplayDialog("확인", "씬의 모든 ObjectDataComponent 오브젝트를 삭제하시겠습니까?", "삭제", "취소"))
            {
                ClearSceneObjects();
            }
        }
        GUI.backgroundColor = Color.white;
    }

    //씬의 모든 ObjectDataComponent 오브젝트를 삭제하는 함수
    private void ClearSceneObjects()
    {
        ObjectDataComponent[] objects = FindObjectsOfType<ObjectDataComponent>();

        int count = 0;
        foreach (var obj in objects)
        {
            DestroyImmediate(obj.gameObject);
            count++;
        }

        Debug.Log("총 " + count + "개의 ObjectDataComponent 오브젝트가 삭제되었습니다.");
    }
}
#endif