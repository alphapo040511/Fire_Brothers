using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isAccessible = true;

    void Start()
    {
        //자체적으로 길을 추가할거 아니라면 필요 없을듯
        //TrailsManager.instance.AddPoint(this);
    }
}
