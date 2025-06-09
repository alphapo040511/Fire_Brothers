using System.Collections.Generic;
using UnityEngine;

public class TrailsManager : MonoBehaviour
{
    public static TrailsManager instance { get; private set; }

    [Header("스테이지 인덱스")]
    public int stageIndex;

    [Header("웨이포인트 프리팹")]
    public GameObject waypointPrefab;

    [Header("로드된 웨이포인트")]
    public List<Waypoint> waypoins = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        LoadWaypoints(GameManager.Instance.currentStageIndex);
    }

    public void AddPoint(Waypoint point)
    {
        waypoins.Add(point);
    }

    public Waypoint GetWaypoint(int index)
    {
        if (waypoins.Count <= index)
        {
            StageStatsManager.Instance.GameEnd();
            return null;
        }

        if(!waypoins[index].isAccessible)       //현재 waypoint가 접근 불가라면 차량 모두 정지
        {
            PlayableObjectsManager.Instance.CanMoveChanged?.Invoke(false);
        }
        
        return waypoins[index];
    }

    public void LoadWaypoints(int index)
    {
        waypoins.Clear();

        WaypointData loaded = WaypointIO.LoadWaypointPositions(index);
        if (loaded == null) return;

        for(int i = 0; i < loaded.points.Count; i++)
        {
            GameObject obj = Instantiate(waypointPrefab, loaded.points[i], Quaternion.identity, transform);
            Waypoint wp = obj.GetComponent<Waypoint>();
            wp.isAccessible = loaded.isAccessible[i];
            waypoins.Add(wp);
        }

        Debug.Log($"웨이포인트 {loaded.points.Count}개 로드됨");
        GameManager.Instance.ChangeState(GameState.Playing);
    }

    private void OnDrawGizmos()
    {
        if (waypoins == null || waypoins.Count < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < waypoins.Count - 1; i++)
        {
            if (waypoins[i] != null && waypoins[i + 1] != null)
            {
                Gizmos.DrawLine(waypoins[i].transform.position, waypoins[i + 1].transform.position);
            }
        }
    }

}
