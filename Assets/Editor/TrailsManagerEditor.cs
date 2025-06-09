#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrailsManager))]
public class TrailsManagerEditor : Editor
{
    private TrailsManager tm;
    private bool isPainting = false;

    private void OnEnable()
    {
        tm = (TrailsManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        if (GUILayout.Button(isPainting ? "웨이포인트 찍기 종료" : "웨이포인트 찍기 시작"))
        {
            isPainting = !isPainting;
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("현재 웨이포인트 저장 (JSON)"))
        {
            WaypointIO.SaveWaypoints(tm.waypoins, tm.stageIndex);
        }

        if (GUILayout.Button("웨이포인트 불러오기 (JSON → Instantiate)"))
        {
            ClearWaypoints();
            LoadWaypoints();
        }
    }

    private void OnSceneGUI()
    {
        if (!isPainting || tm == null || tm.waypointPrefab == null)
            return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(tm.waypointPrefab, tm.transform);
                go.transform.position = hit.point;

                Waypoint wp = go.GetComponent<Waypoint>();
                if (wp != null)
                {
                    tm.AddPoint(wp);
                    Debug.Log($"웨이포인트 생성: {hit.point}");
                }
                else
                {
                    Debug.LogError("생성된 오브젝트에 Waypoint 컴포넌트가 없습니다.");
                }

                e.Use();
            }
        }
    }

    private void ClearWaypoints()
    {
        foreach (var wp in tm.waypoins)
        {
            if (wp != null)
            {
                DestroyImmediate(wp.gameObject);
            }
        }
        tm.waypoins.Clear();
    }

    private void LoadWaypoints()
    {
        var loaded = WaypointIO.LoadWaypointPositions(tm.stageIndex);
        if (loaded != null && tm.waypointPrefab != null)
        {
            for (int i = 0; i < loaded.points.Count; i++)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(tm.waypointPrefab, tm.transform);
                go.transform.position = loaded.points[i];
                Waypoint wp = go.GetComponent<Waypoint>();
                wp.isAccessible = loaded.isAccessible[i];
                tm.waypoins.Add(wp);
            }
            Debug.Log($"웨이포인트 {loaded.points.Count}개 불러오기 완료");
        }
        else
        {
            Debug.LogWarning("웨이포인트 불러오기 실패: 데이터 또는 프리팹 누락");
        }
    }
}
#endif
