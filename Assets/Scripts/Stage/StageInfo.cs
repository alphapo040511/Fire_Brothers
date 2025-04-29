using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public int stageIndex;      //스테이지 번호
    public int[] starScoreThresholds = new int[3]; // 별 점수 기준
    public int unlockRequiredStars; // 해금 필요한 별 수
}

[System.Serializable]
public class StageInfoWrapper       //스테이지 데이터 래퍼 클래스
{
    public List<StageInfo> stages = new();      //스테이지들을 담을 리스트
}

