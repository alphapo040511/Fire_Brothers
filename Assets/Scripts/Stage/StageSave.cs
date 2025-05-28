using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageSave
{
    public int stageIndex;
    public bool isCleared;
    public int bestStars;
    public int highScore;
}

[System.Serializable]
public class StageSaveWrapper
{
    public List<StageSave> saves = new();
    public int totalStars;
}
