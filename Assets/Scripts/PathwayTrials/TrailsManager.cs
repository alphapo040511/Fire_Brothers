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
        LoadWaypoints(stageIndex);
    }

    public void AddPoint(Waypoint point)
    {
        waypoins.Add(point);
    }

    public Waypoint GetWaypoint(int index)
    {
        if (waypoins.Count > index)
        {
            return waypoins[index];
        }

        return null;
    }

    public void LoadWaypoints(int index)
    {
        List<Vector3> loaded = WaypointIO.LoadWaypointPositions(index);
        if (loaded == null) return;

        foreach (var pos in loaded)
        {
            GameObject obj = Instantiate(waypointPrefab, pos, Quaternion.identity, transform);
            Waypoint wp = obj.GetComponent<Waypoint>();
            waypoins.Add(wp);
        }

        Debug.Log($"웨이포인트 {loaded.Count}개 로드됨");
    }
}
