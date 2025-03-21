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

    public Queue<Waypoint> waypoins = new Queue<Waypoint>();

    public void AddPoint(Waypoint point)
    {
        waypoins.Enqueue(point);
    }

    public Waypoint GetWaypoint()
    {
        if(waypoins.Count > 0)
        {
            return waypoins.Dequeue();
        }

        return null;
    }
}
