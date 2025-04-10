using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData
{
    public int stageIndex;
    public bool isCleared;
    public int stars;
    public bool isUnlockedWithoutStars;
}

[System.Serializable]
public class StageDataWrapper
{
    public List<StageData> stages = new();
}

