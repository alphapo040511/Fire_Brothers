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

    public Transform[] targets;                                 //소방차의 transform

    //Start에서 차량 오브젝트 검색 후 중점 구해도 괜찮을 듯
    public bool hasMultipleTargets = false;                     //따라가야 하는 타겟이 여러개인지

    public Vector3 cameraOffset = new Vector3(4, 18, 0);       //카메라의 추적시 위치

    void Start()
    {
        VehicleMove[] temp = FindObjectsOfType<VehicleMove>();

        targets = new Transform[temp.Length];

        for(int i = 0; i < temp.Length; i++)
        {
            targets[i] = temp[i].transform;
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
        if(targets.Length <= 0)
        {
            Debug.LogWarning("차량을 찾을 수 없습니다.");
        }

        Vector3 targetPos = TargetPosition();

        transform.position = targetPos + cameraOffset;
        transform.LookAt(targetPos);
    }

    private Vector3 TargetPosition()
    {
        int count = 0;
        Vector3 cneter = Vector3.zero;
        for (int i = 0; i < targets.Length; i++)
        {
            count++;
            cneter += targets[i].transform.position;
        }

        return cneter / count;
    }

}
