using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractData
{
    public List<HeldItemType> needItems;
    public int maxProgress;
    public HeldItemType rewardItem;

    [System.NonSerialized]
    public int currentProgress = 0;

    public bool ProgressInteraction()
    {
        currentProgress++;

        Debug.Log($"현재 진행도 : {currentProgress}/{maxProgress}");

        if(currentProgress >= maxProgress )
        {
            Debug.Log("상호작용 완료");
            return true;
        }

        return false;
    }
}
