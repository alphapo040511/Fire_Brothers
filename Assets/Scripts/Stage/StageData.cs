using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int stageIndex;      //스테이지 번호
    public bool isCleared;      //클리어 여부
    public int stars;           //해당 스테이지에서 획득한 별
    public bool isUnlockedWithoutStars;     //이 스테이지는 이 전 스테이지에서 별을 획득하지 않아도 열리는 스테이지인지.
}

[System.Serializable]
public class StageDataWrapper       //스테이지 데이터 래퍼 클래스
{
    public List<StageData> stages = new();      //스테이지들을 담을 리스트
}

