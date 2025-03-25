using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsManager : MonoBehaviour
{
    public static TrailsManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public List<Waypoint> waypoins = new List<Waypoint>();

    //웨이포인트를 미리 저장하지 않을 경우 추가하는 함수
    public void AddPoint(Waypoint point)
    {
        waypoins.Add(point);
    }

    public Waypoint GetWaypoint(int index)
    {
        if(waypoins.Count > 0 && waypoins.Count > index)
        {
            return waypoins[index];
        }

        return null;
    }
}
