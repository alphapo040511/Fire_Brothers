using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesInteractable : ProgressInteractable
{
    public LayerMask LayerMask;
    public override void Complite(PlayerInteraction playerData)
    {
        base.Complite(playerData);

        Collider[] waypoints = Physics.OverlapSphere(transform.position, 5f, LayerMask);

        Debug.Log(waypoints.Length + "개의 WayPoint 찾음");

        for (int i = 0; i < waypoints.Length; i++)
        {
            Waypoint way = waypoints[i].GetComponent<Waypoint>();
            if(way != null)
            {
                way.UnlockPath();
                StageStatsManager.Instance.GainScore(40);
            }
        }

        Destroy(gameObject);
    }
}
