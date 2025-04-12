#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrailsManager))]
public class TrailsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrailsManager tm = (TrailsManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("현재 웨이포인트 저장 (JSON)"))
        {
            WaypointIO.SaveWaypoints(tm.waypoins, tm.stageIndex);
        }

        if (GUILayout.Button("웨이포인트 불러오기 (JSON → Instantiate)"))
        {
            // 먼저 기존 웨이포인트 클리어
            foreach (var wp in tm.waypoins)
            {
                if (wp != null) DestroyImmediate(wp.gameObject);
            }

            tm.waypoins.Clear();

            var loaded = WaypointIO.LoadWaypointPositions(tm.stageIndex);
            if (loaded != null && tm.waypointPrefab != null)
            {
                foreach (var pos in loaded)
                {
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(tm.waypointPrefab, tm.transform);
                    go.transform.position = pos;

                    Waypoint wp = go.GetComponent<Waypoint>();
                    tm.waypoins.Add(wp);
                }

                Debug.Log($"웨이포인트 {loaded.Count}개 불러오기 완료");
            }
            else
            {
                Debug.LogWarning("웨이포인트 불러오기 실패: 데이터 또는 프리팹 누락");
            }
        }
    }
}
#endif
