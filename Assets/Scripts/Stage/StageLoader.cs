using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static int nextStageIndex = -1;      //스태틱으로 선언 StageLoader.nextStageIndex = stageIndex; 스테이지 로드 시 이번 스테이지 인덱스를 로딩 인덱스와 동일하게 설정

    void Start()        //스테이지 씬이 시작 될 때
    {
        if (nextStageIndex >= 0)        //StageLoader의 nextStageIndex가 변경되었을 때(입장 했을 때)
        {
            LevelDataConverter.LoadLevelData(nextStageIndex);       //해당 스테이지 JSON에 저장된 오브젝트를 풀링
            Debug.Log($"스테이지 {nextStageIndex} 로드 완료");          //디버그
            nextStageIndex = -1;        //스태틱 리셋
        }
    }
}
