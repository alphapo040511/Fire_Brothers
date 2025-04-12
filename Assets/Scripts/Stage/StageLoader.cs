using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static int nextStageIndex = -1;

    void Start()
    {
        if (nextStageIndex >= 0)
        {
            LevelDataConverter.LoadLevelData(nextStageIndex);
            Debug.Log($"스테이지 {nextStageIndex} 로드 완료");
            nextStageIndex = -1;
        }
    }
}
