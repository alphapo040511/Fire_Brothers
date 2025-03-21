using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isAccessible = true;

    void Start()
    {
        TrailsManager.instance.AddPoint(this);
    }
}
