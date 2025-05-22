using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라의 상태를 나타내는 enum
public enum CameraStats
{
    LoadingView,    // 로딩 중 (혹은 맵 전체 보여주는 중)
    FollowTarget,   // 차량 따라가기
    GameEnd         // 게임 종료 (엔딩 연출용)
}

public class CameraFollow : MonoBehaviour
{
    public CameraStats cameraStats;                             //카메라 상태

    public Transform targets;

    public Vector3 cameraOffset = new Vector3(4, 18, 0);       //카메라의 추적시 위치

    void Start()
    {
        if(targets == null && PlayableObjectsManager.Instance != null)
        {
            targets = PlayableObjectsManager.Instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(cameraStats)
        {
            case CameraStats.FollowTarget:
                FollowVehicle();
                break;
        }
    }

    private void FollowVehicle()
    {
        if (targets == null)
        {
            if (PlayableObjectsManager.Instance != null)
            {
                targets = PlayableObjectsManager.Instance.transform;
            }
            return;
        }

        transform.position = targets.position + cameraOffset;
        transform.LookAt(targets.position);
    }

}
