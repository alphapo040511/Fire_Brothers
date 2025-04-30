#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class LevelDataToolEditor : EditorWindow
{
    private int m_LevelIndex = 1;

    [MenuItem("Tools/Level Data Tool")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataToolEditor>("Level Data Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Data Save / Load / Clear", EditorStyles.boldLabel);

        m_LevelIndex = EditorGUILayout.IntField("Level Index", m_LevelIndex);

        if (GUILayout.Button("Save Level Data"))
        {
            LevelDataConverter.SaveLevelData(m_LevelIndex);
        }

        if (GUILayout.Button("Load Level Data"))
        {
            LevelDataConverter.LoadLevelData(m_LevelIndex);
        }

        GUILayout.Space(10);

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Scene Level Data"))
        {
            if (EditorUtility.DisplayDialog("확인", "씬의 모든 ObjectDataComponent 오브젝트를 삭제하시겠습니까?", "삭제", "취소"))
            {
                ClearSceneObjects();
            }
        }
        GUI.backgroundColor = Color.white;
    }

    private void ClearSceneObjects()
    {
        ObjectDataComponent[] objects = GameObject.FindObjectsOfType<ObjectDataComponent>();
        foreach (var obj in objects)
        {
            if (obj != null)
                DestroyImmediate(obj.gameObject);
        }

        Debug.Log($"ObjectDataComponent 오브젝트 전부 삭제됨");
    }
}
#endif
